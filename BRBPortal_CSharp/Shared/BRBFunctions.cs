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

        private static XmlDocument GetXmlResponse(SoapRequest soapRequest)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var xmlDoc = new XmlDocument();
            var soapMessage = NewSoapMessage();
            var xmlString = "";

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

                    var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                    Logger.Log("SoapRequest URL", urlPrefix + soapRequest.Url);

                    try
                    {
                        request = WebRequest.Create(urlPrefix + soapRequest.Url);
                        request.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                        request.Headers.Add("SOAPAction", soapRequest.Action);
                        request.ContentType = "text/xml; charset=utf-8";
                        request.ContentLength = soapByte.Length;
                        request.Method = "POST";

                        requestStream = request.GetRequestStream();
                        requestStream.Write(soapByte, 0, soapByte.Length);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    try
                    {
                        response = request.GetResponse();
                        responseStream = response.GetResponseStream();
                        reader = new StreamReader(responseStream);

                        xmlString = reader.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

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
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.AppendFormat("<pwd>{0}</pwd>", SafeString(password));
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
            var isConfirmed = false;

            var soapRequest = new SoapRequest
            {
                Source = "ConfirmProfile",
                Url = "ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Port",
                Action = "RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Binder_confirmProfileInformation"
            };

            try
            {
                var userCode = 
                soapRequest.Body.Append("<api:confirmProfileInformation>");
                soapRequest.Body.Append("<profileConfirmationReq>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                //soapRequest.Body.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", user.Question1.EscapeXMLChars());
                //soapRequest.Body.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", user.Answer1.EscapeXMLChars());
                //soapRequest.Body.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", user.Question2.EscapeXMLChars());
                //soapRequest.Body.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", user.Answer2.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<declarationInitial>{0}</declarationInitial>", SafeString(user.DeclarationInitials));
                soapRequest.Body.Append("</profileConfirmationReq>");
                soapRequest.Body.Append("</api:confirmProfileInformation>");
                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("profileDetails"))
                {
                    isConfirmed = string.IsNullOrEmpty(detail.SelectSingleNode("relationship").InnerText);

                    //if (!isConfirmed)
                    //{
                    //    iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                    //}
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
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.AppendFormat("<currentPwd>{0}</currentPwd>", SafeString(currentPassword));
                soapRequest.Body.AppendFormat("<newPwd>{0}</newPwd>", SafeString(newPassword));
                soapRequest.Body.AppendFormat("<retypeNewPwd>{0}</retypeNewPwd>", SafeString(reTypePwd));
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
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode, defaultValue: "?"));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode, defaultValue: "?"));
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
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.Append("<name>");
                soapRequest.Body.AppendFormat("<first>{0}</first>", SafeString(user.FirstName));
                soapRequest.Body.AppendFormat("<!--Optional:--><middle>{0}</middle>", SafeString(user.MiddleName));
                soapRequest.Body.AppendFormat("<last>{0}</last>", SafeString(user.LastName));
                soapRequest.Body.AppendFormat("<suffix>{0}</suffix>", SafeString(user.Suffix));
                soapRequest.Body.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                soapRequest.Body.Append("</name>");
                soapRequest.Body.Append("<mailingAddress>");
                soapRequest.Body.AppendFormat("<!--Optional:--><streetNumber>{0}</streetNumber>", SafeString(user.StreetNumber));
                soapRequest.Body.AppendFormat("<!--Optional:--><streetName>{0}</streetName>", SafeString(user.StreetName));
                soapRequest.Body.AppendFormat("<!--Optional:--><unitNumber>{0}</unitNumber>", SafeString(user.UnitNumber));
                soapRequest.Body.AppendFormat("<!--Optional:--><fullAddress>{0}</fullAddress>", SafeString(user.FullAddress));
                soapRequest.Body.AppendFormat("<!--Optional:--><city>{0}</city>", SafeString(user.City));
                soapRequest.Body.AppendFormat("<!--Optional:--><state>{0}</state>", SafeString(user.StateCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><zip>{0}</zip>", SafeString(user.ZipCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><country>{0}</country>", SafeString(user.Country));
                soapRequest.Body.Append("</mailingAddress>");
                soapRequest.Body.AppendFormat("<emailAddress>{0}</emailAddress>", SafeString(user.Email));
                soapRequest.Body.AppendFormat("<phone>{0}</phone>", user.PhoneNumber);
                soapRequest.Body.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", SafeString(user.Question1));
                soapRequest.Body.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", SafeString(user.Answer1));
                soapRequest.Body.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", SafeString(user.Question2));
                soapRequest.Body.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", SafeString(user.Answer2));
                soapRequest.Body.Append("</updateUserProfileReq>");
                soapRequest.Body.Append("<!--Optional:--><isActive>Y</isActive>");
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
        public static bool Register(ref BRBUser user)
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
                soapRequest.Body.Append("<api:validateRegistrationRequest>");
                soapRequest.Body.Append("<registrationRequestReq>");
                soapRequest.Body.Append("<profileDetails>");
                soapRequest.Body.AppendFormat("<userId>{0}</userId>", user.UserCode.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<billingCode>{0}</billingCode>", user.BillingCode.EscapeXMLChars());
                soapRequest.Body.Append("<name>");
                soapRequest.Body.Append("<!--Optional:--><first></first>");
                soapRequest.Body.Append("<!--Optional:--><middle></middle>");
                soapRequest.Body.AppendFormat("<last>{0}</last>", SafeString(user.LastName));
                soapRequest.Body.Append("<suffix></suffix>");
                soapRequest.Body.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                soapRequest.Body.Append("</name>");
                soapRequest.Body.Append("<mailingAddress>");
                soapRequest.Body.AppendFormat("<!--Optional:--><streetNumber>{0}</streetNumber>", SafeString(user.StreetNumber));
                soapRequest.Body.AppendFormat("<!--Optional:--><streetName>{0}</streetName>", SafeString(user.StreetName));
                soapRequest.Body.Append("<!--Optional:--><unitNumber></unitNumber>");
                soapRequest.Body.Append("<fullAddress></fullAddress>");
                soapRequest.Body.AppendFormat("<!--Optional:--><city>{0}</city>", SafeString(user.City));
                soapRequest.Body.AppendFormat("<!--Optional:--><state>{0}</state>", SafeString(user.StateCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><zip>{0}</zip>", SafeString(user.ZipCode));
                soapRequest.Body.Append("<!--Optional:--><country></country>");
                soapRequest.Body.Append("</mailingAddress>");
                soapRequest.Body.AppendFormat("<emailAddress>{0}</emailAddress>", SafeString(user.Email));
                soapRequest.Body.AppendFormat("<phone>{0}</phone>", SafeString(user.PhoneNumber));
                soapRequest.Body.Append("<securityQuestion1></securityQuestion1>");
                soapRequest.Body.Append("<securityAnswer1></securityAnswer1>");
                soapRequest.Body.Append("<securityQuestion2></securityQuestion2>");
                soapRequest.Body.Append("<securityAnswer2></securityAnswer2>");
                soapRequest.Body.Append("</profileDetails>");
                soapRequest.Body.Append("<propertyDetails>");
                soapRequest.Body.AppendFormat("<relationship>{0}</relationship>", SafeString(user.Relationship));
                soapRequest.Body.AppendFormat("<ownerLastName>{0}</ownerLastName>", SafeString(user.PropertyOwnerLastName));
                soapRequest.Body.AppendFormat("<address>{0}</address>", SafeString(user.PropertyAddress));
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
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
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
#if DEBUG
                                if (currentFees == 0)
                                {
                                    currentFees = 10.0M;
                                }
                                if (totalBalance == 0)
                                {
                                    totalBalance = 10.0M;
                                }
#endif
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
        public static bool GetPropertyUnits(ref BRBUser user, string unitID = "")
        {
            var gotPropertyUnits = true; // later

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
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", SafeString(user.CurrentProperty.PropertyID));
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getPropertyAndUnitDetails>");

                var xmlDoc = GetXmlResponse(soapRequest);

                user.CurrentProperty.Units = new List<BRBUnit>();

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
                    user.CurrentProperty.BillingAddress = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText;

                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        DateTime startDate = DateTime.MinValue;
                        Decimal rentCeiling = 0;
                        var exemptionReason = "";
                        var cpUnitStatDisp = "";
                        var hServices = "";
                        var occupiedBy = "";

                        var unit = new BRBUnit();

                        if ((detailUnits.SelectSingleNode("rentCeiling").InnerText.Length > 0))
                        {
                            Decimal.TryParse(detailUnits.SelectSingleNode("rentCeiling").InnerText, out rentCeiling);
                        }

                        if (detailUnits.SelectSingleNode("unitStatusAsOfDate") != null)
                        {
                            if (!string.IsNullOrEmpty(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText))
                            {
                                DateTime.TryParse(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText, out startDate);
                            }
                        }

                        foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
                        {
                            if (hServices.Length > 0)
                            {
                                hServices += (", " + detailService.SelectSingleNode("serviceName").InnerText);
                            }
                            else
                            {
                                hServices = detailService.SelectSingleNode("serviceName").InnerText;
                            }
                        }

                        switch (unit.UnitStatCode.ToUpper())
                        {
                            case "OOCC":
                                cpUnitStatDisp = "Owner-Occupied";
                                break;
                            case "SEC8":
                                cpUnitStatDisp = "Section 8";
                                break;
                            case "RENTED":
                                cpUnitStatDisp = "Rented or Available for Rent";
                                break;
                            case "FREE":
                                cpUnitStatDisp = "Rent-Free";
                                break;
                            case "NAR":
                                cpUnitStatDisp = "Not Available for Rent";
                                break;
                            case "SPLUS":
                                cpUnitStatDisp = "Shelter Plus";
                                break;
                            case "DUPLEX":
                                cpUnitStatDisp = "Owner-occupied Duplex";
                                break;
                            case "COMM":
                                cpUnitStatDisp = "Commercial";
                                break;
                            case "SHARED":
                                cpUnitStatDisp = "Owner Shares Kit/Bath";
                                break;
                            case "MISC":
                                cpUnitStatDisp = "Miscellaneous Exempt";
                                break;
                        }

                        foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                        {
                            if (occupiedBy.Length > 0)
                            {
                                occupiedBy += (", " + detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText);
                            }
                            else
                            {
                                occupiedBy = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
                            }
                        }

                        switch (unit.UnitStatCode.ToUpper())
                        {
                            case "OOCC":
                                exemptionReason = "Owner-Occupied";
                                break;
                            case "SEC8":
                                exemptionReason = "Section 8";
                                break;
                            case "RENTED":
                                exemptionReason = "Rented or Available for Rent";
                                break;
                            case "FREE":
                                exemptionReason = "Rent-Free";
                                break;
                            case "NAR":
                                exemptionReason = "Not Available for Rent";
                                break;
                            case "SPLUS":
                                exemptionReason = "Shelter Plus";
                                break;
                            case "DUPLEX":
                                exemptionReason = "Owner-occupied Duplex";
                                break;
                            case "COMM":
                                exemptionReason = "Commercial";
                                break;
                            case "SHARED":
                                exemptionReason = "Owner Shares Kit/Bath";
                                break;
                            case "MISC":
                                exemptionReason = "Miscellaneous Exempt";
                                break;
                        }

                        unit.ClientPortalUnitStatusCode = detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText;
                        //unit.CommResYN = "";
                        //unit.CommUseDesc = "";
                        //unit.CommZoneUse = "";
                        //unit.ContractNo = "";
                        unit.CPUnitStatDisp = cpUnitStatDisp;
                        //unit.DatePriorTenancyEnded = "";
                        //unit.DeclarationInitials = "";
                        unit.ExemptionReason = exemptionReason;
                        unit.HServices = hServices;
                        unit.InitialRent = ""; // part of Tenant
                        unit.MultiUnitYN = "";
                        //unit.NumberOfTenants = "";
                        unit.OccupiedBy = occupiedBy;
                        unit.OtherUnits = "";
                        unit.PMEmailPhone = "";
                        unit.PrincResYN = "";
                        unit.PriorEndDate = "";
                        unit.PropMgrName = "";
                        unit.ReasonPriorTenancyEnded = "";
                        unit.RentCeiling = rentCeiling;
                        unit.SmokeDetector = "";
                        //unit.SmokingProhibitionEffectiveDate = "";
                        unit.SmokingProhibitionInLeaseStatus = "";

                        if (startDate != DateTime.MinValue)
                        {
                            unit.StartDt = startDate;
                        }

                        unit.StreetAddress = detailUnits.SelectSingleNode("unitStreetAddress").InnerText;
                        unit.TenantContacts = "";
                        unit.TenantNames = unit.TenantNames;
                        unit.TenantContacts = unit.TenantContacts;
                        unit.TennantCount = 0;
                        unit.TerminationReason = "";
                        //unit.UnitAsOfDt = "";
                        unit.UnitID = detailUnits.SelectSingleNode("unitId").InnerText;
                        unit.UnitNo = detailUnits.SelectSingleNode("unitNumber").InnerText;
                        unit.UnitStatCode = detailUnits.SelectSingleNode("unitStatusCode").InnerText;
                        unit.UnitStatID = detailUnits.SelectSingleNode("unitStatusId").InnerText;

                        if (string.IsNullOrEmpty(unitID) || unitID == unit.UnitID)
                        {
                            user.CurrentProperty.Units.Add(unit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.Message;
                Logger.LogException("GetPropertyUnits", ex);
            }

            return gotPropertyUnits;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static void GetPropertyTenants(ref BRBUser user)
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
                user.CurrentProperty.Tenants = new List<BRBTenant>();

                soapRequest.Body.Append("<api:getPropertyAndUnitDetails>");
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", SafeString(user.CurrentProperty.PropertyID));
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getPropertyAndUnitDetails>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
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

                    user.CurrentUnit.HServices = ""; // so we don't add on to those passed in

                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        var unitID = detailUnits.SelectSingleNode("unitId").InnerText;
                        if (user.CurrentUnit.UnitID == detailUnits.SelectSingleNode("unitId").InnerText)
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
                                DateTime dtSmokingProhibitionEffectiveDate = DateTime.MinValue;

                                if (DateTime.TryParse(detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0).InnerText, out dtSmokingProhibitionEffectiveDate))
                                {
                                    user.CurrentUnit.SmokingProhibitionEffectiveDate = dtSmokingProhibitionEffectiveDate;
                                }
                            }

                            user.CurrentUnit.ClientPortalUnitStatusCode = detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText;

                            foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                            {
                                var tenant = new BRBTenant();

                                tenant.TenantID =  detailOccBy.SelectSingleNode("occupantId").InnerText;
                                tenant.FirstName = detailOccBy.SelectSingleNode("name").SelectSingleNode("firstName").InnerText;
                                tenant.LastName = detailOccBy.SelectSingleNode("name").SelectSingleNode("lastName").InnerText;
                                tenant.DisplayName = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
                                tenant.PhoneNumber = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("phoneNumber").InnerText;
                                tenant.Email = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("emailAddress").InnerText;

                                user.CurrentProperty.Tenants.Add(tenant);
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
                soapRequest.Body.Append("<api:savePaymentCart>");
                soapRequest.Body.Append("<savePaymentToCart>");
                soapRequest.Body.AppendFormat("<cartId>{0}</cartId>", user.Cart.ID);
                soapRequest.Body.AppendFormat("<!--Optional:--><paymentConfirmationNo>{0}</paymentConfirmationNo>", user.Cart.PaymentConfirmationNo);
                soapRequest.Body.AppendFormat("<!--Optional:--><paymentMethod>{0}</paymentMethod>", SafeString(user.Cart.PaymentMethod));
                soapRequest.Body.AppendFormat("<paymentReceivedAmt>{0}</paymentReceivedAmt>", user.Cart.PaymentReceivedAmt);
                soapRequest.Body.AppendFormat("<isFeeOnlyPaid>{0}</isFeeOnlyPaid>", user.Cart.isFeeOnlyPaid ? "true" : "false" );
                soapRequest.Body.Append("<!--1 or more repetitions:-->");

                foreach(var cartItem in user.Cart.Items)
                {
                    soapRequest.Body.Append("<items>");
                    soapRequest.Body.AppendFormat("<itemId>{0}</itemId>", cartItem.ID);
                    soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", SafeString(cartItem.PropertyId));
                    soapRequest.Body.AppendFormat("<propertyMainStreetAddress>{0}</propertyMainStreetAddress>", SafeString(cartItem.PropertyAddress));
                    soapRequest.Body.AppendFormat("<fee>{0}</fee>", cartItem.SumCurrentFees());
                    soapRequest.Body.AppendFormat("<penalties>{0}</penalties>", cartItem.SumPenalties(user.Cart.isFeeOnlyPaid));
                    soapRequest.Body.AppendFormat("<balance>{0}</balance>", cartItem.SumBalance(user.Cart.isFeeOnlyPaid));
                    soapRequest.Body.Append("</items>");
                }

                soapRequest.Body.Append("</savePaymentToCart>");
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
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
        public static bool SaveUnit(ref BRBUser user)
        {
            var wasSaved = false;

            var soapRequest = new SoapRequest
            {
                Source = "SaveUnit",
                Url = "UpdateUnitStatusChange/RTSClientPortalAPI_API_WSD_UpdateUnitStatusChange_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdateUnitStatusChange_Binder_updateUnitStatusChange"
            };

            try
            {
                var unit = user.CurrentUnit;

                soapRequest.Body.Append("<api:updateUnitStatusChange>");
                soapRequest.Body.Append("<unitStatusChangeReq>");
                soapRequest.Body.AppendFormat("<userId>{0}</userId>", user.UserCode);
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", user.CurrentProperty.PropertyID);
                soapRequest.Body.AppendFormat("<unitId>{0}</unitId>", user.CurrentUnit.UnitID);
                soapRequest.Body.AppendFormat("<clientPortalUnitStatusCode>{0}</clientPortalUnitStatusCode>", unit.ClientPortalUnitStatusCode);
                soapRequest.Body.AppendFormat("<unitStatus>{0}</unitStatus>", unit.ClientPortalUnitStatusCode);

                var exemptionReason = "";
                if (unit.ClientPortalUnitStatusCode == "Exempt")
                {
                    if (user.CurrentUnit.ExemptionReason.ToUpper() == "OTHER")
                    {
                        exemptionReason = unit.OtherExemptionReason;
                    }
                    else
                    {
                        exemptionReason = unit.ExemptionReason;
                    }
                }
                soapRequest.Body.AppendFormat("<!--Optional:--><exemptionReason>{0}</exemptionReason>", exemptionReason);

                var asOfDate = "";
                if (unit.UnitAsOfDt.HasValue)
                {
                    asOfDate = unit.UnitAsOfDt.Value.ToString("MM/dd/yyyy");
                } else
                {
                    asOfDate = DateTime.Now.ToString("MM/dd/yyyy");
                }
                soapRequest.Body.AppendFormat("<unitStatusAsOfDate>{0}</unitStatusAsOfDate>", asOfDate);

                soapRequest.Body.AppendFormat("<declarationInitial>{0}</declarationInitial>", unit.DeclarationInitials);

                soapRequest.Body.Append("<questions>");

                if (string.IsNullOrEmpty(unit.ExemptionReason) || !Regex.IsMatch(unit.ExemptionReason, "OOCC|FREE"))
                {
                    soapRequest.Body.Append("<!--Optional:--><asOfDate></asOfDate>");
                }
                else
                {
                    soapRequest.Body.AppendFormat("<!--Optional:--><asOfDate>{0}</asOfDate>", asOfDate);
                }

                DateTime dtDateStarted = DateTime.MinValue;
                if (DateTime.TryParse("", out dtDateStarted))
                {
                    soapRequest.Body.AppendFormat("<!--Optional:--><dateStarted>{0}</dateStarted>", dtDateStarted.ToString("MM/dd/yyyy"));
                }
                else
                {
                    soapRequest.Body.Append("<!--Optional:--><dateStarted></dateStarted>");
                }

                soapRequest.Body.AppendFormat("<!--Optional:--><occupiedBy>{0}</occupiedBy>", unit.OccupiedBy);
                soapRequest.Body.AppendFormat("<!--Optional:--><contractNo>{0}</contractNo>", unit.ContractNo);
                soapRequest.Body.AppendFormat("<!--Optional:--><commeUseDesc>{0}</commeUseDesc>", unit.CommUseDesc);
                soapRequest.Body.AppendFormat("<!--Optional:--><isCommeUseZoned>{0}</isCommeUseZoned>", unit.CommZoneUse);
                soapRequest.Body.AppendFormat("<!--Optional:--><isExclusivelyForCommeUse>{0}</isExclusivelyForCommeUse>", unit.CommResYN);

                // Next 3 removed  when Owner Occupied Exempt Duplex was removed from the Other dropdown
                soapRequest.Body.Append("<!--Optional:--><_x0035_0PercentAsOf31Dec1979></_x0035_0PercentAsOf31Dec1979>");
                soapRequest.Body.Append("<!--Optional:--><ownerOccupantName></ownerOccupantName>"); // OK: vb.net is same
                soapRequest.Body.Append("<!--Zero or more repetitions:--><namesOfownersOfRecord></namesOfownersOfRecord>");

                soapRequest.Body.AppendFormat("<!--Optional:--><nameOfPropertyManagerResiding>{0}</nameOfPropertyManagerResiding>", unit.PropMgrName);
                soapRequest.Body.AppendFormat("<!--Optional:--><emailOfPhoneOfPropertyManagerResiding>{0}</emailOfPhoneOfPropertyManagerResiding>", unit.PMEmailPhone);
                soapRequest.Body.AppendFormat("<!--Optional:--><IsOwnersPrinciplePlaceOfResidence>{0}</IsOwnersPrinciplePlaceOfResidence>", unit.PrincResYN);
                soapRequest.Body.AppendFormat("<!--Optional:--><doesOwnerResideInOtherUnitOfThisUnitProperty>{0}</doesOwnerResideInOtherUnitOfThisUnitProperty>", unit.MultiUnitYN);
                soapRequest.Body.Append("<!--Zero or more repetitions:--><tenantsAndContactInfo>"); // OK: vb.net is same
                soapRequest.Body.AppendFormat("<name>{0}</name>", unit.TenantNames);
                soapRequest.Body.AppendFormat("<contactInfo>{0}</contactInfo>", unit.TenantContacts);
                soapRequest.Body.Append("</tenantsAndContactInfo>");
                soapRequest.Body.Append("</questions>");
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
        public static bool SaveTenant(ref BRBUser user)
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
                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:updateUnitTenancy>");
                soapMessage.Append("<unitTenancyUpdateReq>");
                soapMessage.AppendFormat("<userId>{0}</userId>", user.UserCode);
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", user.CurrentProperty.PropertyID);
                soapMessage.AppendFormat("<unitId>{0}</unitId>", user.CurrentUnit.UnitID);
                soapMessage.AppendFormat("<unitStatus>{0}</unitStatus>", ""); // TODO
                soapMessage.AppendFormat("<initialRent>{0}</initialRent>", ""); // TODO
                soapMessage.AppendFormat("<tenancyStartDate>{0}</tenancyStartDate>", ""); // TODO
                soapMessage.AppendFormat("<priorTenancyEndDate>{0}</priorTenancyEndDate>", ""); // TODO

                soapMessage.Append("<!--Zero or more repetitions:-->");
                // for each housing services
                soapMessage.Append("<housingServices>");
                soapMessage.AppendFormat("<serviceName>{0}</serviceName>", ""); // TODO
                soapMessage.Append("</housingServices>");

                soapMessage.AppendFormat("<!--Optional:--><otherHousingService>{0}</otherHousingService>", ""); // TODO
                soapMessage.AppendFormat("<noOfTenants>{0}</noOfTenants>", ""); // TODO
                soapMessage.AppendFormat("<smokingProhibitionInLeaseStatus>{0}</smokingProhibitionInLeaseStatus>", ""); // TODO
                soapMessage.AppendFormat("<smokingProhibitionEffectiveDate>{0}</smokingProhibitionEffectiveDate>", ""); // TODO
                soapMessage.AppendFormat("<reasonForTermination>{0}</reasonForTermination>", ""); // TODO
                soapMessage.AppendFormat("<otherReasonForTermination>{0}</otherReasonForTermination>", ""); // TODO
                soapMessage.AppendFormat("<!--Optional:--><explainInvoluntaryTermination>{0}</explainInvoluntaryTermination>", ""); // TODO

                soapMessage.Append("<!--Zero or more repetitions:-->");
                // for each tenant
                soapMessage.Append("<tenants>");
                soapMessage.Append("<code></code>");
                soapMessage.Append("<name>");
                soapMessage.AppendFormat("<first>{0}</first>", ""); // TODO
                soapMessage.Append("<!--Optional:--><middle></middle>");
                soapMessage.AppendFormat("<last>{0}</last>", ""); // TODO
                soapMessage.AppendFormat("<suffix>{0}</suffix>", ""); // TODO
                soapMessage.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                soapMessage.Append("<!--Optional:--><agencyName></agencyName>");
                soapMessage.Append("</name>");
                soapMessage.AppendFormat("<phoneNumber>{0}</phoneNumber>", ""); // TODO
                soapMessage.AppendFormat("<emailAddress>{0}</emailAddress>", ""); // TODO
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
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", SafeString(user.Question1));
                soapRequest.Body.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", SafeString(user.Answer1));
                soapRequest.Body.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", SafeString(user.Question2));
                soapRequest.Body.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", SafeString(user.Answer2));
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

        private static string SafeString(string str, string defaultValue = "")
        {
            return string.IsNullOrEmpty(str) ? defaultValue : str.EscapeXMLChars();
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
