using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using Microsoft.AspNet.Identity.Owin;
using BRBPortal_CSharp.Models;
using System.Reflection;
using BRBPortal_CSharp.Shared;
using System.ComponentModel;
using System.Xml.Linq;

namespace BRBPortal_CSharp
{
    public static class BRBFunctions_CSharp
    {
        private const bool USE_MOCK_SERVICES = false;

        public static string iStatus = "";
        public static string iErrMsg = "";

        const string soapNamespace = "http://cityofberkeley.info/RTS/ClientPortal/API";
        const string urlPrefix = "http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.";

        private static StringBuilder NewSoapMessage()
        {
            return new StringBuilder(string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""{0}"">", soapNamespace));
        }
        
        private static Dictionary<string, string> Parse(string soapString)
        {
            var delimiter = new string[] { "::" };
            var obj = new Dictionary<string, string>();

            foreach (var item in soapString.Split(delimiter, StringSplitOptions.None))
            {
                var parts = item.Split('=');
                var key = parts[0];
                var value = parts[1];

                if (!obj.ContainsKey(key))
                {
                    obj.Add(key, value);
                }
            }

            return obj;
        }

        private static XmlDocument GetXmlResponse(SoapRequest soapRequest, bool doNotExecute = false)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var xmlDoc = new XmlDocument();
            var soapMessage = NewSoapMessage();

            iStatus = "";
            iErrMsg = "";

            try
            {
                if (USE_MOCK_SERVICES && !string.IsNullOrEmpty(soapRequest.StaticDataFile))
                {
                    xmlDoc = GetStaticXml(soapRequest.StaticDataFile);
                }
                else
                {
                    soapMessage.Append("<soapenv:Header/>");
                    soapMessage.Append("<soapenv:Body>");
                    soapMessage.Append(soapRequest.Body.ToString());
                    soapMessage.Append("</soapenv:Body>");
                    soapMessage.Append("</soapenv:Envelope>");

                    Logger.Log(string.Format("{0}(request)", soapRequest.Source), soapMessage.ToString());

                    if (doNotExecute)
                    {
                        return xmlDoc;
                    }

                    var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                    request = WebRequest.Create(urlPrefix + soapRequest.Url);
                    request.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                    request.Headers.Add("SOAPAction", soapRequest.Action);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.ContentLength = soapByte.Length;
                    request.Method = "POST";

                    requestStream = request.GetRequestStream();
                    requestStream.Write(soapByte, 0, soapByte.Length);

                    response = request.GetResponse();
                    responseStream = response.GetResponseStream();
                    reader = new StreamReader(responseStream);

                    var xmlString = reader.ReadToEnd();

                    Logger.Log(string.Format("{0}(response)", soapRequest.Source), xmlString);

                    xmlDoc.LoadXml(xmlString);
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException(soapRequest.Action, ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (requestStream != null)
                {
                    requestStream.Close();
                    requestStream.Dispose();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }

            return xmlDoc;
        }

        public static XmlDocument GetStaticXml(string fileName)
        {
            var xmlDoc = new XmlDocument();

            try
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                var filePath = Path.Combine(Path.GetDirectoryName(path), "StaticData", fileName);
                xmlDoc.Load(filePath);
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("GetStaticXml", ex);
            }

            return xmlDoc;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static SignInStatus UserAuth(ref BRBUser user, string password)
        {
            var signInStatus = SignInStatus.Failure;

            var soapRequest = new SoapRequest
            {
                Source = "UserAuth",
                StaticDataFile = "AuthenticateUser_Response.xml",
                Url = "AuthenticateUser/RTSClientPortalAPI_API_WSD_AuthenticateUser_Port",
                Action = "RTSClientPortalAPI_API_WSD_AuthenticateUser_Binder_authenticateUserLogin"
            };

            try
            {
                soapRequest.Body.Append("<api:authenticateUserLogin>");
                soapRequest.Body.Append("<authenticateUserReq>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<pwd>{0}</pwd>", password.EscapeXMLChars());
                soapRequest.Body.Append("</authenticateUserReq>");
                soapRequest.Body.Append("</api:authenticateUserLogin>");

                var xmlDoc = GetXmlResponse(soapRequest);
                
                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("authenticateUserRes"))
                {
                    if (detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS"))
                    {
                        signInStatus = SignInStatus.Success;

                        if (detail.SelectSingleNode("relationship") != null)
                        {
                            user.Relationship = detail.SelectSingleNode("relationship").InnerText;
                        }

                        user.IsFirstlogin = detail.SelectSingleNode("isFirstLogin").InnerText.ToUpper() == "TRUE";
                        user.IsTemporaryPassword = detail.SelectSingleNode("isTemporaryPwd").InnerText.ToUpper() == "TRUE";
                    }
                    else
                    {
                        iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("UserAuth", ex);
            }

            return signInStatus;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ConfirmProfile(BRBUser user)
        {
            if (USE_MOCK_SERVICES)
            {
                iStatus = "SUCCESS";
                iErrMsg = "";
                return true;
            }
            else
            {
                return ConfirmProfile_Soap(user);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ConfirmProfile_Soap(BRBUser user)
        {
            var isConfirmed = false;

            var soapRequest = new SoapRequest
            {
                Source = "ConfirmProfile",
                Url = "AuthenticateUser/RTSClientPortalAPI_API_WSD_AuthenticateUser_Port",
                Action = "ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Port"
            };

            try
            {
                soapRequest.Body.Append("<api:confirmProfileInformation>");
                soapRequest.Body.Append("<profileConfirmationReq>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", user.Question1);
                soapRequest.Body.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", user.Answer1);
                soapRequest.Body.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", user.Question2);
                soapRequest.Body.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", user.Answer2);
                soapRequest.Body.AppendFormat("<declarationInitial>{0}</declarationInitial>", user.DeclarationInitials.EscapeXMLChars());
                soapRequest.Body.Append("</profileConfirmationReq>");
                soapRequest.Body.Append("</api:confirmProfileInformation>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    isConfirmed = detail.SelectSingleNode("status").InnerText.ToUpper().Equals("SUCCESS");

                    if (!isConfirmed)
                    {
                        iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("ConfirmProfile", ex);
            }

            return isConfirmed;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdatePassword(BRBUser user, string currentPassword, string newPassword, string reTypePwd)
        {
            if (USE_MOCK_SERVICES)
            {
                iErrMsg = "";
                return true; // TODO: need sample xml
            }
            else
            {
                return UpdatePassword_Soap(user, currentPassword, newPassword, reTypePwd);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdatePassword_Soap(BRBUser user, string currentPassword, string newPassword, string reTypePwd)
        {
            var wasUpdated = false;

            var soapRequest = new SoapRequest
            {
                Source = "UpdatePassword",
                Url = "UpdatePassword/RTSClientPortalAPI_API_WSD_UpdatePassword_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdatePassword_Binder_updateUserPassword"
            };

            try
            {
                soapRequest.Body.Append("<api:updateUserPassword>");
                soapRequest.Body.Append("<updateUserPwdReq>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<currentPwd>{0}</currentPwd>", currentPassword.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<newPwd>{0}</newPwd>", newPassword.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<retypeNewPwd>{0}</retypeNewPwd>", reTypePwd);
                soapRequest.Body.Append("</updateUserPwdReq>");
                soapRequest.Body.Append("</api:updateUserPassword>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasUpdated = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasUpdated)
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("UpdatePassword", ex);
            }

            return wasUpdated;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool GetProfile(ref BRBUser user)
        {
            var gotProfile = false;

            var soapRequest = new SoapRequest
            {
                Source = "GetProfile",
                StaticDataFile = "GetProfileDetails_Response.xml",
                Url = "GetUserProfile/RTSClientPortalAPI_API_WSD_GetUserProfile_Port",
                Action = "RTSClientPortalAPI_API_WSD_GetUserProfile_Binder_getProfileDetails"
            };

            try
            {
                soapRequest.Body.Append("<api:getProfileDetails>");
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "?" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "?" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getProfileDetails>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("profileDetails"))
                {
                    foreach (XmlElement detailName in xmlDoc.DocumentElement.GetElementsByTagName("name"))
                    {
                        foreach (XmlElement detailAddr in xmlDoc.DocumentElement.GetElementsByTagName("mailingAddress"))
                        {
                            user.BillingCode = detail.SelectSingleNode("billingCode").InnerText;
                            user.Relationship = detail.SelectSingleNode("relationship").InnerText;

                            // Name

                            user.FirstName = detailName.SelectSingleNode("first").InnerText.UnescapeXMLChars();
                            user.LastName = detailName.SelectSingleNode("last").InnerText.UnescapeXMLChars();
                            user.MiddleName = detailName.SelectSingleNode("middle").InnerText;
                            user.Suffix = detailName.SelectSingleNode("suffix").InnerText;
                            user.Email = detail.SelectSingleNode("emailAddress").InnerText;
                            user.PhoneNumber = detail.SelectSingleNode("phone").InnerText;

                            // Address

                            user.StreetNumber = detailAddr.SelectSingleNode("streetNumber").InnerText.UnescapeXMLChars();
                            user.StreetName = detailAddr.SelectSingleNode("streetName").InnerText.UnescapeXMLChars();
                            user.UnitNumber =  detailAddr.SelectSingleNode("unitNumber").InnerText;
                            user.FullAddress = detailAddr.SelectSingleNode("fullAddress").InnerText;
                            user.City = detailAddr.SelectSingleNode("city").InnerText;
                            user.StateCode = detailAddr.SelectSingleNode("state").InnerText;
                            user.ZipCode = detailAddr.SelectSingleNode("zip").InnerText;
                            user.Country = detailAddr.SelectSingleNode("country").InnerText;

                            //var securityAnswer1Node = detail.SelectSingleNode("securityAnswer1");
                            //var securityAnswer2Node = detail.SelectSingleNode("securityAnswer2");

                            //user.Question1 = detail.SelectSingleNode("securityQuestion1").InnerText;
                            //user.Question2 = detail.SelectSingleNode("securityQuestion2").InnerText;

                            //if (securityAnswer1Node != null)
                            //{
                            //    user.Answer1 = securityAnswer1Node.InnerText.UnescapeXMLChars();
                            //}

                            //if (securityAnswer2Node != null)
                            //{
                            //    user.Answer2 = securityAnswer2Node.InnerText.UnescapeXMLChars();
                            //}

                            //if ((detailName.SelectSingleNode("agencyName").InnerText.Length > 0))
                            //{
                            //    user.AgencyName = detailName.SelectSingleNode("agencyName").InnerText.UnescapeXMLChars();
                            //}

                            gotProfile = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("GetProfile", ex);
            }

            return gotProfile;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdateProfile(ref BRBUser user)
        {
            if (USE_MOCK_SERVICES)
            {
                iErrMsg = "";
                return true;
            }
            else
            {
                return UpdateProfile_Soap(ref user);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdateProfile_Soap(ref BRBUser user)
        {
            var wasUpdated = false;

            var soapRequest = new SoapRequest
            {
                Source = "UpdateProfile",
                Url = "UpdateUserProfile/RTSClientPortalAPI_API_WSD_UpdateUserProfile_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdateUserProfile_Binder_updateUserProfile"
            };

            try
            {
                soapRequest.Body.Append("<api:updateUserProfile>");
                soapRequest.Body.Append("<updateUserProfileReq>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.Append("<name>");
                soapRequest.Body.AppendFormat("<first>{0}</first>", user.FirstName.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><middle>{0}</middle>", user.MiddleName.Length == 0 ? "" : user.MiddleName.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<last>{0}</last>", user.LastName.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<suffix>{0}</suffix>", user.Suffix);
                soapRequest.Body.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                soapRequest.Body.AppendFormat("<!--Optional:--><agencyName>{0}</agencyName>", user.AgencyName.Length == 0 ? "" : user.AgencyName.EscapeXMLChars());
                soapRequest.Body.Append("</name>");
                soapRequest.Body.Append("<mailingAddress>");
                soapRequest.Body.AppendFormat("<!--Optional:--><streetNumber>{0}</streetNumber>", user.StreetNumber.Length == 0 ? "" : user.StreetNumber);
                soapRequest.Body.AppendFormat("<!--Optional:--><streetName>{0}</streetName>", user.StreetName.Length == 0 ? "" : user.StreetName.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><unitNumber>{0}</unitNumber>", user.UnitNumber.Length == 0 ? "" : user.UnitNumber);
                soapRequest.Body.AppendFormat("<fullAddress>{0}</fullAddress>", user.FullAddress.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><city>{0}</city>", user.City);
                soapRequest.Body.AppendFormat("<!--Optional:--><state>{0}</state>", user.StateCode);
                soapRequest.Body.AppendFormat("<!--Optional:--><zip>{0}</zip>", user.ZipCode);
                soapRequest.Body.AppendFormat("<!--Optional:--><country>{0}</country>", user.Country);
                soapRequest.Body.Append("</mailingAddress>");
                soapRequest.Body.AppendFormat("<emailAddress>{0}</emailAddress>", user.Email);
                soapRequest.Body.AppendFormat("<phone>{0}</phone>", user.PhoneNumber);
                soapRequest.Body.Append("</updateUserProfileReq>");
                soapRequest.Body.Append("<isActive>Y</isActive>");
                soapRequest.Body.Append("</api:updateUserProfile>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasUpdated = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasUpdated)
                    {
                        iErrMsg = detail.ChildNodes[2].InnerText;
                    }
                }

                // For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("profileDetails")
                //     For Each detailName As XmlElement In doc.DocumentElement.GetElementsByTagName("name")
                //         For Each detailAddr As XmlElement In doc.DocumentElement.GetElementsByTagName("mailingAddress")
                //             iStatus = "SUCCESS"
                //         Next detailAddr
                //     Next detailName
                // Next detail
            }
            catch (Exception ex)
            {
                iStatus = "FAILURE";
                iErrMsg = ex.Message;
                Logger.LogException("UpdateProfile", ex);
            }

            return wasUpdated;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool Register(UserProfile profile)
        {
            bool wasRegistered = false;

            var soapRequest = new SoapRequest
            {
                Source = "Register",
                Url = "ValidateRegistrationRequest/RTSClientPortalAPI_API_WSD_ValidateRegistrationRequest_Port",
                Action = "RTSClientPortalAPI.API.WSD.ValidateRegistrationRequest"
            };

            try
            {
                var userCode = profile.UserCode;
                var billCode = profile.BillingCode;
                var firstName = profile.FirstName;
                var lastName = profile.LastName;
                var email = profile.Email;
                var streetNum = profile.StreetNumber;
                var streetName = profile.StreetName;
                var city = profile.City;
                var state = profile.State;
                var zip = profile.Zip;
                var phone = profile.PhoneNo;
                var agencyName = profile.AgencyName;
                var relationship = profile.Relationship;
                var ownerLastName = profile.PropertyOwnerLastName;
                var propertyAddress = profile.PropertyAddress;

                soapRequest.Body.Append("<api:validateRegistrationRequest>");
                soapRequest.Body.Append("<registrationRequestReq>");
                soapRequest.Body.Append("<profileDetails>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.EscapeXMLChars());
                soapRequest.Body.Append("<name>");
                soapRequest.Body.Append("<!--Optional:--><first></first>");
                soapRequest.Body.Append("<!--Optional:--><middle></middle>");
                soapRequest.Body.AppendFormat("<last>{0}</last>", lastName.EscapeXMLChars());
                soapRequest.Body.Append("<suffix></suffix>");
                soapRequest.Body.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                soapRequest.Body.AppendFormat("<!--Optional:--><agencyName>{0}</agencyName>", agencyName.Length == 0 ? "" : agencyName.EscapeXMLChars());
                soapRequest.Body.Append("</name>");
                soapRequest.Body.Append("<mailingAddress>");
                soapRequest.Body.AppendFormat("<!--Optional:--><streetNumber>{0}</streetNumber>", streetNum);
                soapRequest.Body.AppendFormat("<!--Optional:--><streetName>{0}</streetName>", streetName.EscapeXMLChars());
                soapRequest.Body.Append("<!--Optional:--><unitNumber></unitNumber>");
                soapRequest.Body.Append("<fullAddress></fullAddress>");
                soapRequest.Body.AppendFormat("<!--Optional:--><city>{0}</city>", city);
                soapRequest.Body.AppendFormat("<!--Optional:--><state>{0}</state>", state);
                soapRequest.Body.AppendFormat("<!--Optional:--><zip>{0}</zip>", zip);
                soapRequest.Body.Append("<!--Optional:--><country></country>");
                soapRequest.Body.Append("</mailingAddress>");
                soapRequest.Body.AppendFormat("<emailAddress>{0}</emailAddress>", email);
                soapRequest.Body.AppendFormat("<phone>{0}</phone>", phone);
                soapRequest.Body.Append("</profileDetails>");
                soapRequest.Body.Append("<propertyDetails>");
                soapRequest.Body.AppendFormat("<relationship>{0}</relationship>", relationship);
                soapRequest.Body.AppendFormat("<ownerLastName>{0}</ownerLastName>", ownerLastName.Length == 0 ? "" : ownerLastName);
                soapRequest.Body.AppendFormat("<address>{0}</address>", propertyAddress);
                soapRequest.Body.Append("<purchaseYear></purchaseYear>");
                soapRequest.Body.Append("</propertyDetails>");
                soapRequest.Body.Append("</registrationRequestReq>");
                soapRequest.Body.Append("</api:validateRegistrationRequest>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    iStatus = detail.ChildNodes[0].InnerText;

                    if (iStatus.ToUpper() != "SUCCESS")
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }
                }

                wasRegistered = iStatus.ToUpper().Equals("SUCCESS");
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("Register", ex);
            }

            return wasRegistered;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool GetUserProperties(ref BRBUser user)
        {
            var gotUserProperties = false;

            var soapRequest = new SoapRequest
            {
                Source = "GetUserProperties",
                StaticDataFile = "GetUserProperties_Response.xml",
                Url = "GetUserProfilePropertiesList/RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Port",
                Action = "RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Binder_getProfileProperties"
            };

            try
            {
                soapRequest.Body.Append("<api:getProfileProperties>");
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "?" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "?" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getProfileProperties>");

                var xmlDoc = GetXmlResponse(soapRequest);
                user.Properties = new List<BRBProperty>();

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    foreach (XmlElement detailProperty in detail.GetElementsByTagName("profileProperty"))
                    {
                        foreach (XmlElement balanceAmounts in detailProperty.GetElementsByTagName("balanceAmounts"))
                        {
                            decimal currentFees = 0;
                            decimal priorFees = 0;
                            decimal currentPenalty = 0;
                            decimal priorPenalty = 0;
                            decimal credit = 0;
                            decimal totalBalance = 0;
                            var myProperty = new BRBProperty();

                            Decimal.TryParse(balanceAmounts.SelectSingleNode("currentFees").InnerText, out currentFees);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("priorFees").InnerText, out priorFees);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("currentPenalty").InnerText, out currentPenalty);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("priorPenalties").InnerText, out priorPenalty);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("credit").InnerText, out credit);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("totalBalance").InnerText, out totalBalance);

                            gotUserProperties = detail.SelectSingleNode("status").InnerText.ToUpper().Equals("SUCCESS");

                            if (!gotUserProperties)
                            {
                                iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                            }
                            else
                            {
                                myProperty.PropertyID = detailProperty.SelectSingleNode("propertyId").InnerText;
                                myProperty.PropertyAddress = detailProperty.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText;
                                myProperty.CurrentFee = currentFees;
                                myProperty.PriorFee = priorFees;
                                myProperty.CurrentPenalty = currentPenalty;
                                myProperty.PriorPenalty = priorPenalty;
                                myProperty.Credits = credit;
                                myProperty.Balance = totalBalance;

                                user.Properties.Add(myProperty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("GetUserProperties", ex);
            }

            return gotUserProperties;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static void GetPropertyUnits(ref BRBUser user, string unitID = "")
        {
            var soapRequest = new SoapRequest
            {
                Source = "GetPropertyUnits",
                StaticDataFile = "GetPropertyAndUnitDetails_Response.xml",
                Url = "GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port",
                Action = "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails"
            };

            try
            {
                soapRequest.Body.Append("<api:getPropertyAndUnitDetails>");
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", user.CurrentProperty.PropertyID);
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "?" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "?" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getPropertyAndUnitDetails>");

                var xmlDoc = GetXmlResponse(soapRequest);

                user.CurrentProperty.Units = new List<BRBUnit>();

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
                    //iPropAddr = "";
                    //iBillAddr = "";
                    //iAgentName = "";
                    //iStatus = "SUCCESS";

                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        DateTime startDate;
                        Decimal rentCeiling = 0;
                        var myUnit = new BRBUnit();

                        myUnit.UnitID = detailUnits.SelectSingleNode("unitId").InnerText;
                        myUnit.UnitNo = detailUnits.SelectSingleNode("unitNumber").InnerText;
                        myUnit.UnitStatID = detailUnits.SelectSingleNode("unitStatusId").InnerText;
                        myUnit.UnitStatCode = detailUnits.SelectSingleNode("unitStatusCode").InnerText;
                        myUnit.StreetAddress = detailUnits.SelectSingleNode("unitStreetAddress").InnerText;
                        myUnit.ClientPortalUnitStatusCode = detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText;

                        if ((detailUnits.SelectSingleNode("rentCeiling").InnerText.Length > 0))
                        {
                            Decimal.TryParse(detailUnits.SelectSingleNode("rentCeiling").InnerText, out rentCeiling);
                        }

                        myUnit.RentCeiling = rentCeiling;

                        if (detailUnits.SelectSingleNode("unitStatusAsOfDate") != null)
                        {
                            if (!string.IsNullOrEmpty(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText))
                            {
                                DateTime.TryParse(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText, out startDate);

                                if (startDate != null)
                                {
                                    myUnit.StartDt = startDate;
                                }
                            }
                        }

                        foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
                        {
                            if (myUnit.HServices.Length > 0)
                            {
                                myUnit.HServices += (", " + detailService.SelectSingleNode("serviceName").InnerText);
                            }
                            else
                            {
                                myUnit.HServices = detailService.SelectSingleNode("serviceName").InnerText;
                            }
                        }

                        switch (myUnit.UnitStatCode.ToUpper())
                        {
                            case "OOCC":
                                myUnit.CPUnitStatDisp = "Owner-Occupied";
                                break;
                            case "SEC8":
                                myUnit.CPUnitStatDisp = "Section 8";
                                break;
                            case "RENTED":
                                myUnit.CPUnitStatDisp = "Rented or Available for Rent";
                                break;
                            case "FREE":
                                myUnit.CPUnitStatDisp = "Rent-Free";
                                break;
                            case "NAR":
                                myUnit.CPUnitStatDisp = "Not Available for Rent";
                                break;
                            case "SPLUS":
                                myUnit.CPUnitStatDisp = "Shelter Plus";
                                break;
                            case "DUPLEX":
                                myUnit.CPUnitStatDisp = "Owner-occupied Duplex";
                                break;
                            case "COMM":
                                myUnit.CPUnitStatDisp = "Commercial";
                                break;
                            case "SHARED":
                                myUnit.CPUnitStatDisp = "Owner Shares Kit/Bath";
                                break;
                            case "MISC":
                                myUnit.CPUnitStatDisp = "Miscellaneous Exempt";
                                break;
                        }

                        foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                        {
                            if (myUnit.OccupiedBy.Length > 0)
                            {
                                myUnit.OccupiedBy += (", " + detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText);
                            }
                            else
                            {
                                myUnit.OccupiedBy = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
                            }
                        }

                        if (!string.IsNullOrEmpty(myUnit.OccupiedBy))
                        {
                            myUnit.OccupiedBy = myUnit.OccupiedBy.UnescapeXMLChars();
                        }

                        switch (myUnit.UnitStatCode.ToUpper())
                        {
                            case "OOCC":
                                myUnit.ExemptionReason = "Owner-Occupied";
                                break;
                            case "SEC8":
                                myUnit.ExemptionReason = "Section 8";
                                break;
                            case "RENTED":
                                myUnit.ExemptionReason = "Rented or Available for Rent";
                                break;
                            case "FREE":
                                myUnit.ExemptionReason = "Rent-Free";
                                break;
                            case "NAR":
                                myUnit.ExemptionReason = "Not Available for Rent";
                                break;
                            case "SPLUS":
                                myUnit.ExemptionReason = "Shelter Plus";
                                break;
                            case "DUPLEX":
                                myUnit.ExemptionReason = "Owner-occupied Duplex";
                                break;
                            case "COMM":
                                myUnit.ExemptionReason = "Commercial";
                                break;
                            case "SHARED":
                                myUnit.ExemptionReason = "Owner Shares Kit/Bath";
                                break;
                            case "MISC":
                                myUnit.ExemptionReason = "Miscellaneous Exempt";
                                break;
                        }

                        if (string.IsNullOrEmpty(unitID) || unitID == myUnit.UnitID)
                        {
                            user.CurrentProperty.Units.Add(myUnit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("GetPropertyUnits", ex);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static void GetPropertyTenants(ref BRBUser user, string propertyID, string unitID)
        {
            var soapRequest = new SoapRequest
            {
                Source = "GetPropertyTenants",
                StaticDataFile = "GetPropertyAndUnitDetails_Response.xml",
                Url = "GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port",
                Action = "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails"
            };

            try
            {
                soapRequest.Body.Append("<api:getPropertyAndUnitDetails>");
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", propertyID);
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "?" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "?" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getPropertyAndUnitDetails>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
                    if (detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress") != null)
                    {
                        user.CurrentProperty.MainStreetAddress = detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress") != null)
                    {
                        user.CurrentProperty.BillingAddress = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay") != null)
                    {
                        user.CurrentProperty.OwnerContactName = detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay").InnerText.UnescapeXMLChars();
                    }

                    if (detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress") != null)
                    {
                        user.CurrentProperty.BillingEmail = detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("agentDetails") != null)
                    {
                        if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName") != null)
                        {
                            user.AgencyName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText.UnescapeXMLChars();
                        }

                        if (user.AgencyName.Length < 1)
                        {
                            if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName") != null)
                            {
                                user.AgencyName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName").InnerText.UnescapeXMLChars();
                            }
                        }
                    }

                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        user.CurrentUnit = new BRBUnit();

                        if (unitID == detailUnits.SelectSingleNode("unitId").InnerText)
                        {
                            if (detailUnits.SelectSingleNode("tenancyStartDate") != null)
                            {
                                if (!string.IsNullOrEmpty(detailUnits.SelectSingleNode("tenancyStartDate").InnerText))
                                {
                                    user.CurrentUnit.StartDt = DateTime.Parse(detailUnits.SelectSingleNode("tenancyStartDate").InnerText);
                                }
                            }

                            foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
                            {
                                if ((user.CurrentUnit.HServices.Length > 0))
                                {
                                    user.CurrentUnit.HServices += (", " + detailService.SelectSingleNode("serviceName").InnerText);
                                }
                                else
                                {
                                    user.CurrentUnit.HServices = detailService.SelectSingleNode("serviceName").InnerText;
                                }
                            }

                            if (detailUnits.GetElementsByTagName("noOfOccupants").Item(0) != null)
                            {
                                user.CurrentUnit.TennantCount = int.Parse(detailUnits.GetElementsByTagName("noOfOccupants").Item(0).InnerText);
                            }

                            if (detailUnits.GetElementsByTagName("initialRent").Item(0) != null)
                            {
                                user.CurrentUnit.InitialRent = detailUnits.GetElementsByTagName("initialRent").Item(0).InnerText;
                            }

                            if (detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0) != null)
                            {
                                user.CurrentUnit.DatePriorTenancyEnded = DateTime.Parse(detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0).InnerText);
                            }

                            if (detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0) != null)
                            {
                                user.CurrentUnit.ReasonPriorTenancyEnded = detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0).InnerText;
                            }

                            if (detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0) != null)
                            {
                                user.CurrentUnit.SmokingProhibitionInLeaseStatus = detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0).InnerText;
                            }

                            if (detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0) != null)
                            {
                                user.CurrentUnit.SmokingProhibitionEffectiveDate = DateTime.Parse(detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0).InnerText);
                            }

                            user.CurrentUnit.ClientPortalUnitStatusCode = detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText;

                            foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                            {
                                user.CurrentTenant = new BRBTenant();

                                user.CurrentTenant.TenantID =  detailOccBy.SelectSingleNode("occupantId").InnerText;
                                user.CurrentTenant.FirstName = detailOccBy.SelectSingleNode("name").SelectSingleNode("firstName").InnerText;
                                user.CurrentTenant.LastName = detailOccBy.SelectSingleNode("name").SelectSingleNode("lastName").InnerText;
                                user.CurrentTenant.DisplayName = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
                                user.CurrentTenant.PhoneNumber = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("phoneNumber").InnerText;
                                user.CurrentTenant.Email = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("emailAddress").InnerText;
                            }

                            //fields.Add("NumTenants", TenCnt.ToString());
                            //fields.Add("SmokeYN", tSmokYN);
                            //fields.Add("SmokeDt", tSmokDt);
                            //fields.Add("InitRent", tInitRent);
                            //fields.Add("PriorEndDt", tPriorDt);
                            //fields.Add("TermReason", tPriorReas);
                            //fields.Add("OwnerName", iBillContact);
                            //fields.Add("AgenntName", iAgentName);
                            //fields.Add("UnitID", detailUnits.SelectSingleNode("unitId").InnerText);
                            //fields.Add("OwnerEmail", iBillEmail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("GetPropertyTenants", ex);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool SaveCart(BRBUser user)
        {
            var wasSaved = false;

            var soapRequest = new SoapRequest
            {
                Source = "SaveCart",
                StaticDataFile = "SaveCart_Response.xml",
                Url = "SavePaymentCartDetails/RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Port",
                Action = "RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Binder_savePaymentCart"
            };

            try
            {
                var userCode = user.UserCode;
                var billCode = user.BillingCode;

                soapRequest.Body.Append("<api:savePaymentCart>");
                soapRequest.Body.Append("<savePaymentToCart>");
                soapRequest.Body.Append("<cartId></cartId>");
                soapRequest.Body.Append("<paymentConfirmationNo/>");
                soapRequest.Body.Append("<paymentReceivedAmt/>");

                if (true) // AllFees
                {
                    soapRequest.Body.AppendFormat("<isFeeOnlyPaid>{0}</isFeeOnlyPaid>", "True");
                }
                else
                {
                    soapRequest.Body.AppendFormat("<isFeeOnlyPaid>{0}</isFeeOnlyPaid>", "False");
                }

                var itemIndex = 0;
                foreach (var item in user.Cart.Items)
                {
                    //item.CurrentFee = 154.99M;
                    //item.PriorFee = 0;
                    //item.Credits = 0;

                    //item.CurrentPenalty = 0;
                    //item.PriorPenalty = 0;
                    //item.Balance = 154.99M;

                    soapRequest.Body.Append("<items>");
                    soapRequest.Body.AppendFormat("<itemId>{0}</itemId>", ++itemIndex);
                    soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", item.PropertyId);
                    soapRequest.Body.AppendFormat("<propertyMainStreetAddress>{0}</propertyMainStreetAddress>", item.PropertyAddress);
                    
                    //soapRequest.Body.AppendFormat("<fee>{0}</fee>", item.CurrentFee + item.PriorFee + item.Credits);
                    soapRequest.Body.AppendFormat("<fee>{0}</fee>", 154.99M);

                    //if (true) // AllFees
                    //{
                    //    soapRequest.Body.AppendFormat("<penalties>{0}</penalties>", item.CurrentPenalty + item.PriorPenalty);
                    //}
                    //else
                    //{
                    //    soapRequest.Body.Append("<penalties>0.00</penalties>");
                    //}
                    soapRequest.Body.Append("<penalties>0.00</penalties>");

                    //soapRequest.Body.AppendFormat("<balance>{0}</balance>", item.Balance);
                    soapRequest.Body.AppendFormat("<balance>{0}</balance>", 154.99M);

                    soapRequest.Body.Append("</items>");
                }

                soapRequest.Body.Append("</savePaymentToCart>");
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.Length == 0 ? "?" : user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "?" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:savePaymentCart>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasSaved = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasSaved)
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }
                }

                if (wasSaved)
                {
                    uint cartID;
                    foreach (XmlElement el in xmlDoc.DocumentElement.GetElementsByTagName("cartId"))
                    {
                        if (uint.TryParse(el.InnerText, out cartID))
                        {
                            user.Cart.ID = cartID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("SaveCart", ex);
            }

            return wasSaved;
        }

        /// <summary>
        /// DONE, need to update MyProperties/UpdateUnit.aspx.cs
        /// </summary>
        public static bool SaveUnit(BRBUser user)
        {
            var wasSaved = false;

            var soapRequest = new SoapRequest
            {
                Source = "SaveUnit",
                Url = "UpdateUnitTenancy/RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Binder_updateUnitTenancy"
            };

            try
            {
                soapRequest.Body.Append("<api:updateUnitStatusChange>");
                soapRequest.Body.Append("<unitStatusChangeReq>");
                soapRequest.Body.AppendFormat("<userId>{0}</userId>", "from session");
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", "from session");
                soapRequest.Body.AppendFormat("<unitId>{0}</unitId>", "from form");
                soapRequest.Body.AppendFormat("<clientPortalUnitStatusCode>{0}</clientPortalUnitStatusCode>", "from form");
                soapRequest.Body.AppendFormat("<unitStatus>{0}</unitStatus>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><exemptionReason>{0}</exemptionReason>", "from form");
                soapRequest.Body.AppendFormat("<unitStatusAsOfDate>{0}</unitStatusAsOfDate>", "from form");
                soapRequest.Body.AppendFormat("<declarationInitial>{0}</declarationInitial>", "from form");
                soapRequest.Body.Append("<questions>");
                soapRequest.Body.AppendFormat("<!--Optional:--><asOfDate>{0}</asOfDate>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><dateStarted>{0}</dateStarted>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><occupiedBy>{0}</occupiedBy>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><contractNo>{0}</contractNo>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><commeUseDesc>{0}</commeUseDesc>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><isCommeUseZoned>{0}</isCommeUseZoned>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><isExclusivelyForCommeUse>{0}</isExclusivelyForCommeUse>", "from form");
               
                // Next 3 removed  when Owner Occupied Exempt Duplex was removed from the Other dropdown
                soapRequest.Body.Append("<!--Optional:--><_x0035_0PercentAsOf31Dec1979></_x0035_0PercentAsOf31Dec1979>");
                soapRequest.Body.Append("<!--Optional:--><ownerOccupantName></ownerOccupantName>");
                soapRequest.Body.Append("<!--Zero or more repetitions:--><namesOfownersOfRecord></namesOfownersOfRecord>");

                soapRequest.Body.AppendFormat("<!--Optional:--><nameOfPropertyManagerResiding>{0}</nameOfPropertyManagerResiding>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><emailOfPhoneOfPropertyManagerResiding>{0}</emailOfPhoneOfPropertyManagerResiding>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><IsOwnersPrinciplePlaceOfResidence>{0}</IsOwnersPrinciplePlaceOfResidence>", "from form");
                soapRequest.Body.AppendFormat("<!--Optional:--><doesOwnerResideInOtherUnitOfThisUnitProperty>{0}</doesOwnerResideInOtherUnitOfThisUnitProperty>", "from form");
                soapRequest.Body.Append("<!--Zero or more repetitions:--><tenantsAndContactInfo>");
                soapRequest.Body.AppendFormat("<name>{0}</name>", "from form");
                soapRequest.Body.AppendFormat("<contactInfo>{0}</contactInfo>", "from form");
                soapRequest.Body.Append("</tenantsAndContactInfo>");
                soapRequest.Body.Append("<questions>");
                soapRequest.Body.Append("</unitStatusChangeReq>");
                soapRequest.Body.Append("</api:updateUnitStatusChange>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasSaved = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasSaved)
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("SaveUnit", ex);
            }

            return wasSaved;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool SaveTenant(string soapString)
        {
            if (USE_MOCK_SERVICES)
            {
                iErrMsg = "";
                return true;
            }
            else
            {
                return SaveTenant_Soap(soapString);
            }
        }

        /// <summary>
        /// DONE, need to update MyProperties/UpdateTenancy.aspx.cs
        /// </summary>
        public static bool SaveTenant_Soap(string soapString)
        {
            var wasSaved = false;

            var soapRequest = new SoapRequest
            {
                Source = "SaveTenant",
                Url = "UpdateUnitTenancy/RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Binder_updateUnitTenancy"
            };

            try
            {
                var fields = Parse(soapString);

                var userCode = fields.GetStringValue("UserID");
                var billCode = fields.GetStringValue("BillingCode");

                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:updateUnitTenancy>");
                soapMessage.Append("<unitTenancyUpdateReq>");
                soapMessage.AppendFormat("<userId>{0}</userId>", "from session");
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", "from session");
                soapMessage.AppendFormat("<unitId>{0}</unitId>", "from form");
                soapMessage.AppendFormat("<unitStatus>{0}</unitStatus>", "from form");
                soapMessage.AppendFormat("<initialRent>{0}</initialRent>", "from form");
                soapMessage.AppendFormat("<tenancyStartDate>{0}</tenancyStartDate>", "from form");
                soapMessage.AppendFormat("<priorTenancyEndDate>{0}</priorTenancyEndDate>", "from form");

                soapMessage.Append("<!--Zero or more repetitions:-->");
                // for each housing services
                soapMessage.Append("<housingServices>");
                soapMessage.AppendFormat("<serviceName>{0}</serviceName>", "from form");
                soapMessage.Append("</housingServices>");

                soapMessage.AppendFormat("<!--Optional:--><otherHousingService>{0}</otherHousingService>", "from form");
                soapMessage.AppendFormat("<noOfTenants>{0}</noOfTenants>", "from form");
                soapMessage.AppendFormat("<smokingProhibitionInLeaseStatus>{0}</smokingProhibitionInLeaseStatus>", "from form");
                soapMessage.AppendFormat("<smokingProhibitionEffectiveDate>{0}</smokingProhibitionEffectiveDate>", "from form");
                soapMessage.AppendFormat("<reasonForTermination>{0}</reasonForTermination>", "from form");
                soapMessage.AppendFormat("<otherReasonForTermination>{0}</otherReasonForTermination>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><explainInvoluntaryTermination>{0}</explainInvoluntaryTermination>", "from form");

                soapMessage.Append("<!--Zero or more repetitions:-->");
                // for each tenant
                soapMessage.Append("<tenants>");
                soapMessage.Append("<code></code>");
                soapMessage.Append("<name>");
                soapMessage.AppendFormat("<first>{0}</first>", "from form");
                soapMessage.Append("<!--Optional:--><middle></middle>");
                soapMessage.AppendFormat("<last>{0}</last>", "from form");
                soapMessage.AppendFormat("<suffix>{0}</suffix>", "from form");
                soapMessage.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                soapMessage.Append("<!--Optional:--><agencyName></agencyName>");
                soapMessage.Append("</name>");
                soapMessage.AppendFormat("<phoneNumber>{0}</phoneNumber>", "from form");
                soapMessage.AppendFormat("<emailAddress>{0}</emailAddress>", "from form");
                soapMessage.Append("</tenants>");

                soapMessage.Append("</unitTenancyUpdateReq>");
                soapMessage.Append("</api:updateUnitTenancy>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    iStatus = detail.ChildNodes[0].InnerText;

                    if (iStatus.ToUpper() != "SUCCESS")
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }
                }

                wasSaved = iStatus.ToUpper().Equals("SUCCESS");
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("SaveTenant", ex);
            }

            return wasSaved;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ValidateReset(ref BRBUser user)
        {
            if (USE_MOCK_SERVICES)
            {
                iErrMsg = "";
                return true; // TODO: need mock object
            }
            else
            {
                return ValidateReset_Soap(ref user);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ValidateReset_Soap(ref BRBUser user)
        {
            var canReset = false;

            var soapRequest = new SoapRequest
            {
                Source = "ValidateReset",
                Url = "ValidateResetPasswordRequest/RTSClientPortalAPI_API_WSD_ValidateResetPasswordRequest_Port",
                Action = "RTSClientPortalAPI_API_WSD_ValidateResetPasswordRequest_Binder_validateResetUserPassword"
            };

            try
            {
                soapRequest.Body.Append("<api:validateResetUserPassword>");
                soapRequest.Body.Append("<resetUserPwdReq>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", user.BillingCode.Length == 0 ? "" : user.BillingCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", user.Question1.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", user.Answer1.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", user.Question2.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", user.Answer2.EscapeXMLChars());
                soapRequest.Body.Append("</resetUserPwdReq>");
                soapRequest.Body.Append("</api:validateResetUserPassword>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    canReset = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!canReset)
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("ValidateReset", ex);
            }

            return canReset;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();
            var properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }
            return table;
        }

        public static bool CheckPswdRules(BRBUser user, string password)
        {
            bool RetValue = false;
            bool tResult = true;
            bool testB;
            Regex RegexObj = new Regex("[a-zA-Z0-9!@#$%^&_*]");

            // Test Length
            if ((password.Length < 7 || password.Length > 20))
            {
                tResult = false;
            }

            testB = false;
            if (RegexObj.IsMatch(password))
            {
                testB = true;
            }

            if ((testB == false))
            {
                tResult = false;
            }

            testB = false;
            if (password.IndexOf(user.UserCode) > -1)
            {
                testB = true;
            }

            if ((testB == true))
            {
                tResult = false;
            }

            RetValue = tResult;
            return RetValue;
        }

        /// <summary>
        /// C# equivalent of VB.NET Left() function.
        /// </summary>
        public static string Left(string str, int length)
        {
            return str.Substring(0, length);
        }

        /// <summary>
        /// C# equivalent of VB.NET Right() function.
        /// </summary>
        public static string Right(string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// C# equivalent of VB.NET Mid() function.
        /// </summary>
        public static string Mid(string str, int startIndex, int length = 0)
        {
            if (length == 0)
            {
                return Right(str, str.Length - startIndex);
            }
            else
            {
                return str.Substring(startIndex - 1, length);
            }
        }
    }
}
