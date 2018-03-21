using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using Microsoft.AspNet.Identity.Owin;
using BRBPortal_CSharp.Models;

//using QSILib;

namespace BRBPortal_CSharp
{
    public static class BRBFunctions_CSharp
    {
        private const bool USE_MOCK_SERVICES = false;

        public static string iStatus = "";
        public static string iRelate = "";
        public static string iErrMsg = "";
        public static string iFirstlogin = "";
        public static string iTempPwd = "";
        public static string iPropAddr = "";
        public static string iAgentName = "";
        public static string iBillContact = "";
        public static string iBillAddr = "";
        public static string iBillEmail = "";

        public static DataTable iPropertyTbl = new DataTable();
        public static DataTable iUnitsTbl = new DataTable();
        public static DataTable iTenantsTbl = new DataTable();

        const string soapNamespace = "http://cityofberkeley.info/RTS/ClientPortal/API";
        const string uriPrefix = "http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.";

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

        /// <summary>
        /// DONE
        /// </summary>
        public static SignInStatus UserAuth(string userCode, string billCode, string password)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                iRelate = "";
                iTempPwd = "temp";
                iStatus = "SUCCESS";
                iFirstlogin = "TRUE";

                return SignInStatus.Success;
            }
            else
            {
                return UserAuth_Soap(userCode, billCode, password);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        private static SignInStatus UserAuth_Soap(string userCode, string billCode, string password)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var signInStatus = SignInStatus.Failure;

            try
            {
                var xmlDoc = new XmlDocument();
                var soapMessage = NewSoapMessage();

                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:authenticateUserLogin>");
                soapMessage.Append("<authenticateUserReq>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.Length == 0 ? "?" : userCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.Length == 0 ? "?" : billCode.EscapeXMLChars());
                soapMessage.AppendFormat("<pwd>{0}</pwd>", password.EscapeXMLChars());
                soapMessage.Append("</authenticateUserReq>");
                soapMessage.Append("</api:authenticateUserLogin>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "AuthenticateUser/RTSClientPortalAPI_API_WSD_AuthenticateUser_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_AuthenticateUser_Binder_authenticateUserLogin");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                var childeNodes = xmlDoc.DocumentElement.GetElementsByTagName("authenticateUserRes");

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("authenticateUserRes"))
                {
                    // Set session variables from response

                    iStatus = detail.ChildNodes[0].InnerText;

                    if (iStatus.ToUpper().Equals("SUCCESS"))
                    {
                        if (detail.SelectSingleNode("relationship") == null)
                        {
                            iRelate = "";
                        }
                        else
                        {
                            iRelate = detail.SelectSingleNode("relationship").InnerText;
                        }

                        iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                        iTempPwd = detail.SelectSingleNode("isTemporaryPwd").InnerText;
                        iFirstlogin = detail.SelectSingleNode("isFirstLogin").InnerText;
                    }
                    else
                    {
                        iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                        iRelate = "";
                        iTempPwd = "";
                        iFirstlogin = "";
                    }
                }

                if (iStatus.ToUpper().Equals("SUCCESS"))
                {
                    signInStatus = SignInStatus.Success;
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
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

            return signInStatus;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ConfirmProfile(string aID, string aBillCd, string aInits)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                iStatus = "SUCCESS";
                return true;
            }
            else
            {
                return ConfirmProfile_Soap(aID, aBillCd, aInits);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ConfirmProfile_Soap(string aID, string aBillCd, string aInits)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var isConfirmed = false;

            try
            {
                var xmlDoc = new XmlDocument();
                var soapMessage = NewSoapMessage();

                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:confirmProfileInformation>");
                soapMessage.Append("<profileConfirmationReq>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", aID.Length == 0 ? "?" : aID.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", aBillCd.Length == 0 ? "?" : aBillCd.EscapeXMLChars());
                soapMessage.AppendFormat("<declarationInitial>{0}</declarationInitial>", aInits.EscapeXMLChars());
                soapMessage.Append("</profileConfirmationReq>");
                soapMessage.Append("</api:confirmProfileInformation>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Porthttp://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Porthttp://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ConfirmUserProfileByDeclaration/RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_ConfirmUserProfileByDeclaration_Binder_confirmProfileInformation");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);
                requestStream.Close();

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    iStatus = detail.SelectSingleNode("status").InnerText;

                    if (iStatus.ToUpper() != "SUCCESS")
                    {
                        iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                    }
                }

                isConfirmed = iStatus.ToUpper().Equals("SUCCESS");
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
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

            return isConfirmed;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdatePassword(string userCode, string billCode, string currentPassword, string newPassword, string reTypePwd)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return true; // TODO: need sample xml
            }
            else
            {
                return UpdatePassword_Soap(userCode, billCode, currentPassword, newPassword, reTypePwd);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdatePassword_Soap(string userCode, string billCode, string currentPassword, string newPassword, string reTypePwd)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var wasUpdated = false;

            try
            {
                var xmlDoc = new XmlDocument();
                var soapMessage = NewSoapMessage();

                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:updateUserPassword>");
                soapMessage.Append("<updateUserPwdReq>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.Length == 0 ? "?" : userCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.Length == 0 ? "?" : billCode.EscapeXMLChars());
                soapMessage.AppendFormat("<currentPwd>{0}</currentPwd>", currentPassword.EscapeXMLChars());
                soapMessage.AppendFormat("<newPwd>{0}</newPwd>", newPassword.EscapeXMLChars());
                soapMessage.AppendFormat("<retypeNewPwd>{0}</retypeNewPwd>", reTypePwd);
                soapMessage.Append("</updateUserPwdReq>");
                soapMessage.Append("</api:updateUserPassword>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "UpdatePassword/RTSClientPortalAPI_API_WSD_UpdatePassword_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdatePassword_Binder_updateUserPassword");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    iStatus = detail.ChildNodes[0].InnerText;

                    if (iStatus.ToUpper() != "SUCCESS")
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }
                }

                wasUpdated = iStatus.ToUpper().Equals("SUCCESS");
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
                return false;
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

            return wasUpdated;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static Dictionary<string, string> GetProfile(string userCode, string billCode)
        {
            var responseString = string.Empty;

            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                var fields = new Dictionary<string, string>();

                iStatus = "SUCCESS";

                fields.Add("UserCode", "UC");
                fields.Add("BillingCode", "BC");
                fields.Add("FirstName", "David");
                fields.Add("LastName", "Balmer");
                fields.Add("FullName", "David Balmer");
                fields.Add("StNum", "123");
                fields.Add("StName", "Main");
                fields.Add("Unit", "");
                fields.Add("City", "Alameda");
                fields.Add("State", "CA");
                fields.Add("Zip", "94501");
                fields.Add("Country", "USA");
                fields.Add("FullAddr", "123 Main Street, Alameda, CA 94501 USA");
                fields.Add("Email", "david.b.balmer@transsight.com");
                fields.Add("Phone", "555-555-5555");
                fields.Add("Question1", "What is your name?");
                fields.Add("Answer1", "Dave");
                fields.Add("Question2", "Is your name Dave?");
                fields.Add("Answer2", "Yes");
                fields.Add("MidName", "");
                fields.Add("Suffix", "");
                fields.Add("AgentName", "secret");
                fields.Add("MailAddr", "123 Main Street, Alameda, CA 94501 USA");

                return fields;
            }
            else
            {
                return GetProfile_Soap(userCode, billCode);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        private static Dictionary<string, string> GetProfile_Soap(string userCode, string billCode)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var fields = new Dictionary<string, string>();

            iStatus = "";

            try
            {
                var xmlDoc = new XmlDocument();
                var soapMessage = NewSoapMessage();

                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:getProfileDetails>");
                soapMessage.Append("<request>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.Length == 0 ? "?" : userCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.Length == 0 ? "?" : billCode.EscapeXMLChars());
                soapMessage.Append("</request>");
                soapMessage.Append("</api:getProfileDetails>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "GetUserProfile/RTSClientPortalAPI_API_WSD_GetUserProfile_Por");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetUserProfile_Binder_getProfileDetails");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);
                requestStream.Close();

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("profileDetails"))
                {
                    foreach (XmlElement detailName in xmlDoc.DocumentElement.GetElementsByTagName("name"))
                    {
                        foreach (XmlElement detailAddr in xmlDoc.DocumentElement.GetElementsByTagName("mailingAddress"))
                        {
                            iStatus = "SUCCESS";

                            fields.Add("UserCode", detail.SelectSingleNode("userId").InnerText);
                            fields.Add("BillingCode", detail.SelectSingleNode("billingCode").InnerText);
                            fields.Add("FirstName", detailName.SelectSingleNode("first").InnerText.UnescapeXMLChars());
                            fields.Add("LastName", detailName.SelectSingleNode("last").InnerText.UnescapeXMLChars());
                            fields.Add("StNum", detailAddr.SelectSingleNode("streetNumber").InnerText.UnescapeXMLChars());
                            fields.Add("StName", detailAddr.SelectSingleNode("streetName").InnerText.UnescapeXMLChars());
                            fields.Add("Unit", detailAddr.SelectSingleNode("unitNumber").InnerText);
                            fields.Add("FullAddr", detailAddr.SelectSingleNode("fullAddress").InnerText);
                            fields.Add("City", detailAddr.SelectSingleNode("city").InnerText);
                            fields.Add("State", detailAddr.SelectSingleNode("state").InnerText);
                            fields.Add("Zip", detailAddr.SelectSingleNode("zip").InnerText);
                            fields.Add("Country", detailAddr.SelectSingleNode("country").InnerText);
                            fields.Add("Email", detail.SelectSingleNode("emailAddress").InnerText);
                            fields.Add("Phone", detail.SelectSingleNode("phone").InnerText);
                            fields.Add("Question1", detail.SelectSingleNode("securityQuestion1").InnerText);
                            fields.Add("Answer1", detail.SelectSingleNode("securityAnswer1").InnerText.UnescapeXMLChars());
                            fields.Add("Question2", detail.SelectSingleNode("securityQuestion2").InnerText);
                            fields.Add("Answer2", detail.SelectSingleNode("securityAnswer2").InnerText.UnescapeXMLChars());

                            if ((detailName.SelectSingleNode("middle").InnerText.Length > 0))
                            {
                                fields.Add("MidName", detailName.SelectSingleNode("middle").InnerText);
                            }

                            if ((detailName.SelectSingleNode("suffix").InnerText.Length > 0))
                            {
                                fields.Add("Suffix", detailName.SelectSingleNode("suffix").InnerText);
                            }

                            if ((detailName.SelectSingleNode("agencyName").InnerText.Length > 0))
                            {
                                fields.Add("AgentName", detailName.SelectSingleNode("agencyName").InnerText.UnescapeXMLChars());
                            }

                            // Build Full Name
                            var middleName = fields.GetStringValue("MidName");

                            fields.Add("FullName", string.Format("{0}{1} {2} {3}",
                                fields.GetStringValue("FirstName"),
                                string.IsNullOrEmpty(middleName) ? "" : (" " + middleName),
                                fields.GetStringValue("LastName"),
                                fields.GetStringValue("Sufix")
                            ));

                            // Build Mailing address

                            var mailAddress = detailAddr.SelectSingleNode("streetNumber").InnerText;

                            mailAddress += (" " + detailAddr.SelectSingleNode("streetName").InnerText);

                            if (detailAddr.SelectSingleNode("unitNumber").InnerText.Length > 0)
                            {
                                mailAddress += (", " + detailAddr.SelectSingleNode("unitNumber").InnerText);
                            }

                            mailAddress += (", " + detailAddr.SelectSingleNode("city").InnerText);
                            mailAddress += (", " + detailAddr.SelectSingleNode("state").InnerText);
                            mailAddress += (" " + detailAddr.SelectSingleNode("zip").InnerText);

                            fields.Add("MailAddr", mailAddress.UnescapeXMLChars());
                        }
                    }
                }

                if (iStatus.ToUpper() != "SUCCESS")
                {
                    // Should be blank since the API failed
                    iStatus = "FAILURE";
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
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

            return fields;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdateProfile(UserProfile profile)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return true;
            }
            else
            {
                return UpdateProfile_Soap(profile);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdateProfile_Soap(UserProfile profile)
        {
            // SEE Test/Live Data/GetProfile_Response.txt for sample data

            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var wasUpdated = false;

            iStatus = "";

            try
            {
                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:updateUserProfile>");
                soapMessage.Append("<updateUserProfileReq>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", profile.UserCode.Length == 0 ? "" : profile.UserCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", profile.BillingCode.Length == 0 ? "" : profile.BillingCode.EscapeXMLChars());
                soapMessage.Append("<name>");
                soapMessage.AppendFormat("<first>{0}</first>", profile.FirstName.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><middle>{0}</middle>", profile.MiddleName.Length == 0 ? "" : profile.MiddleName.EscapeXMLChars());
                soapMessage.AppendFormat("<last>{0}</last>", profile.LastName.EscapeXMLChars());
                soapMessage.AppendFormat("<suffix>{0}</suffix>", profile.Suffix);
                soapMessage.Append("<!--Optional:--><nameLastFirstDisplay>{0}</nameLastFirstDisplay>");
                soapMessage.AppendFormat("<!--Optional:--><agencyName>{0}</agencyName>", profile.AgencyName.Length == 0 ? "" : profile.AgencyName.EscapeXMLChars());
                soapMessage.Append("</name>");
                soapMessage.Append("<mailingAddress>");
                soapMessage.AppendFormat("<!--Optional:--><streetNumber>{0}</streetNumber>", profile.StreetNumber.Length == 0 ? "" : profile.StreetNumber);
                soapMessage.AppendFormat("<!--Optional:--><streetName>{0}</streetName>", profile.StreetName.Length == 0 ? "" : profile.StreetName.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><unitNumber>{0}</unitNumber>", profile.Unit.Length == 0 ? "" : profile.Unit);
                soapMessage.AppendFormat("<fullAddress>{0}</fullAddress>", profile.FullAddress.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><city>{0}</city>", profile.City);
                soapMessage.AppendFormat("<!--Optional:--><state>{0}</state>", profile.State);
                soapMessage.AppendFormat("<!--Optional:--><zip>{0}</zip>", profile.Zip);
                soapMessage.AppendFormat("<!--Optional:--><country>{0}</country>", profile.Country);
                soapMessage.Append("</mailingAddress>");
                soapMessage.AppendFormat("<emailAddress>{0}</emailAddress>", profile.Email);
                soapMessage.AppendFormat("<phone>{0}</phone>", profile.PhoneNo);
                soapMessage.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", profile.Question1);
                soapMessage.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", profile.Answer1);
                soapMessage.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", profile.Question2);
                soapMessage.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", profile.Answer2);
                soapMessage.Append("</updateUserProfileReq>");
                soapMessage.Append("<isActive>Y</isActive>");
                soapMessage.Append("</api:updateUserProfile>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var xmlDoc = new XmlDocument();
                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "UpdateUserProfile/RTSClientPortalAPI_API_WSD_UpdateUserProfile_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUserProfile_Binder_updateUserProfile");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    iStatus = detail.ChildNodes[0].InnerText;

                    if (iStatus.ToUpper() == "FAILURE")
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

                wasUpdated = iStatus.ToUpper().Equals("SUCCESS");

                if (!wasUpdated)
                {
                    iStatus = "FAILURE";
                }
            }
            catch (Exception ex)
            {
                iStatus = "FAILURE";
                iErrMsg = ex.ToString();
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

            return wasUpdated;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool Register(string soapString)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return true;
            }
            else
            {
                return Register(soapString);
            }
        }

        /// <summary>
        /// DONE, need to update Register.aspx.cs
        /// </summary>
        public static bool Register_Soap(string soapString)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var wasRegistered = false;

            try
            {
                var fields = Parse(soapString);

                var userCode = fields.GetStringValue("UserID");
                var billCode = fields.GetStringValue("BillingCode");
                var fullName = fields.GetStringValue("FullName");
                var firstName = fields.GetStringValue("FirstName");
                var middleName = fields.GetStringValue("MidName");
                var lastName = fields.GetStringValue("LastName");
                var mailAddr = fields.GetStringValue("MailAddr");
                var streetNum = fields.GetStringValue("StNum");
                var streetName = fields.GetStringValue("StName");
                var unit = fields.GetStringValue("Unit");
                var fullAddress = fields.GetStringValue("FullAddr");
                var city = fields.GetStringValue("City");
                var state = fields.GetStringValue("State");
                var zip = fields.GetStringValue("Zip");
                var country = fields.GetStringValue("Country");
                var email = fields.GetStringValue("Email");
                var phone = fields.GetStringValue("Phone");
                var answer1 = fields.GetStringValue("Answer1");
                var answer2 = fields.GetStringValue("Answer2");
                var question1 = fields.GetStringValue("Question1");
                var question2 = fields.GetStringValue("Question2");
                var suffix = fields.GetStringValue("Suffix");
                var agentName = fields.GetStringValue("AgentName");

                // TODO: need to verify field names
                var relationship = fields.GetStringValue("Relationship");
                var ownerLastName = fields.GetStringValue("OwnerLastName");
                var propertyAddress = fields.GetStringValue("PropertyAddress");
                var purchaseYear = fields.GetStringValue("PurchaseYear");

                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:validateRegistrationRequest>");
                soapMessage.Append("<registrationRequestReq>");
                soapMessage.Append("<profileDetails>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.Length == 0 ? "?" : userCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.Length == 0 ? "?" : billCode.EscapeXMLChars());
                soapMessage.Append("<name>");
                soapMessage.AppendFormat("<first>{0}</first>", firstName.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><middle>{0}</middle>", middleName.Length == 0 ? "?" : middleName.EscapeXMLChars());
                soapMessage.AppendFormat("<last>{0}</last>", lastName.EscapeXMLChars());
                soapMessage.AppendFormat("<suffix>{0}</suffix>", suffix);
                soapMessage.Append("<!--Optional:--><nameLastFirstDisplay>{0}</nameLastFirstDisplay>");
                soapMessage.AppendFormat("<!--Optional:--><agencyName>{0}</agencyName>", agentName.Length == 0 ? "?" : agentName.EscapeXMLChars());
                soapMessage.Append("</name>");
                soapMessage.Append("<mailingAddress>");
                soapMessage.AppendFormat("<!--Optional:--><streetNumber>{0}</streetNumber>", streetNum.Length == 0 ? "?" : streetNum);
                soapMessage.AppendFormat("<!--Optional:--><streetName>{0}</streetName>", streetName.Length == 0 ? "?" : streetName.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><unitNumber>{0}</unitNumber>", unit.Length == 0 ? "?" : unit);
                soapMessage.AppendFormat("<fullAddress>{0}</fullAddress>", fullAddress.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><city>{0}</city>", city);
                soapMessage.AppendFormat("<!--Optional:--><state>{0}</state>", state);
                soapMessage.AppendFormat("<!--Optional:--><zip>{0}</zip>", zip);
                soapMessage.AppendFormat("<!--Optional:--><country>{0}</country>", country);
                soapMessage.Append("</mailingAddress>");
                soapMessage.AppendFormat("<emailAddress>{0}</emailAddress>", email);
                soapMessage.AppendFormat("<phone>{0}</phone>", phone);
                soapMessage.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", question1);
                soapMessage.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", answer1);
                soapMessage.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", question2);
                soapMessage.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", answer2);
                soapMessage.Append("</profileDetails>");
                soapMessage.Append("<propertyDetails>");
                soapMessage.AppendFormat("<relationship>{0}</relationship>", relationship);
                soapMessage.AppendFormat("<ownerLastName>{0}</ownerLastName>", ownerLastName);
                soapMessage.AppendFormat("<address>{0}</address>", propertyAddress);
                soapMessage.AppendFormat("<purchaseYear>{0}</purchaseYear>", purchaseYear);
                soapMessage.Append("</propertyDetails>");
                soapMessage.Append("</registrationRequestReq>");
                soapMessage.Append("</api:validateRegistrationRequest>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var xmlDoc = new XmlDocument();
                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "ValidateRegistrationRequest/RTSClientPortalAPI_API_WSD_ValidateRegistrationRequest_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI.API.WSD.ValidateRegistrationRequest");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

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
                iErrMsg = ex.ToString();
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

            return wasRegistered;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool GetUserProperties(string userCode, string billCode)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                if (iPropertyTbl.Columns.Count < 1)
                {
                    iPropertyTbl.Columns.Add("chkProp", typeof(string));
                    iPropertyTbl.Columns.Add("PropertyID", typeof(string));
                    iPropertyTbl.Columns.Add("MainAddr", typeof(string));
                    iPropertyTbl.Columns.Add("CurrFees", typeof(Decimal));
                    iPropertyTbl.Columns.Add("PriorFees", typeof(Decimal));
                    iPropertyTbl.Columns.Add("CurrPenalty", typeof(Decimal));
                    iPropertyTbl.Columns.Add("PriorPenalty", typeof(Decimal));
                    iPropertyTbl.Columns.Add("Credits", typeof(Decimal));
                    iPropertyTbl.Columns.Add("Balance", typeof(Decimal));
                    iPropertyTbl.Columns.Add("btnUpdProp", typeof(string));
                }

                // Clear out any properties from prior call
                if (iPropertyTbl.Rows.Count > 0)
                {
                    iPropertyTbl.Clear();
                }

                DataRow NR = iPropertyTbl.NewRow();
                NR.SetField<string>("PropertyID", "123");
                NR.SetField<string>("MainAddr", "123 Main Street, Anytown, USA 99999");
                NR.SetField<Decimal>("CurrFees", 1.11M);
                NR.SetField<Decimal>("PriorFees", 2.22M);
                NR.SetField<Decimal>("CurrPenalty", 3.33M);
                NR.SetField<Decimal>("PriorPenalty", 4.44M);
                NR.SetField<Decimal>("Credits", 5.55M);
                NR.SetField<Decimal>("Balance", 6.66M);
                iPropertyTbl.Rows.Add(NR);

                return true;
            }
            else
            {
                return GetUserProperties_Soap(userCode, billCode);
            }
        }
        
        /// <summary>
        /// DONE
        /// </summary>
        public static bool GetUserProperties_Soap(string userCode, string billCode)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var gotUserProperties = false;

            iStatus = "";

            if (iPropertyTbl.Columns.Count < 1)
            {
                iPropertyTbl.Columns.Add("chkProp", typeof(string));
                iPropertyTbl.Columns.Add("PropertyID", typeof(string));
                iPropertyTbl.Columns.Add("MainAddr", typeof(string));
                iPropertyTbl.Columns.Add("CurrFees", typeof(Decimal));
                iPropertyTbl.Columns.Add("PriorFees", typeof(Decimal));
                iPropertyTbl.Columns.Add("CurrPenalty", typeof(Decimal));
                iPropertyTbl.Columns.Add("PriorPenalty", typeof(Decimal));
                iPropertyTbl.Columns.Add("Credits", typeof(Decimal));
                iPropertyTbl.Columns.Add("Balance", typeof(Decimal));
                iPropertyTbl.Columns.Add("btnUpdProp", typeof(string));
            }

            // Clear out any properties from prior call
            if (iPropertyTbl.Rows.Count > 0)
            {
                iPropertyTbl.Clear();
            }

            try
            {
                var xmlDoc = new XmlDocument();
                var soapMessage = NewSoapMessage();

                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:getProfileProperties>");
                soapMessage.Append("<request>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.Length == 0 ? "?" : userCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.Length == 0 ? "?" : billCode.EscapeXMLChars());
                soapMessage.Append("</request>");
                soapMessage.Append("</api:getProfileProperties>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "GetUserProfilePropertiesList/RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Binder_getProfileProperties");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    foreach (XmlElement detailProperty in detail.GetElementsByTagName("profileProperty"))
                    {
                        foreach (XmlElement detailAmounts in detailProperty.GetElementsByTagName("balanceAmounts"))
                        {
                            decimal currentFees = 0;
                            decimal priorFees = 0;
                            decimal currentPenalty = 0;
                            decimal priorPenalty = 0;
                            decimal credit = 0;
                            decimal totalBalance = 0;

                            Decimal.TryParse(detailAmounts.SelectSingleNode("currentFees").InnerText, out currentFees);
                            Decimal.TryParse(detailAmounts.SelectSingleNode("priorFees").InnerText, out priorFees);
                            Decimal.TryParse(detailAmounts.SelectSingleNode("currentPenalty").InnerText, out currentPenalty);
                            Decimal.TryParse(detailAmounts.SelectSingleNode("priorPenalties").InnerText, out priorPenalty);
                            Decimal.TryParse(detailAmounts.SelectSingleNode("credit").InnerText, out credit);
                            Decimal.TryParse(detailAmounts.SelectSingleNode("totalBalance").InnerText, out totalBalance);

                            iStatus = detail.SelectSingleNode("status").InnerText;

                            if (iStatus.ToUpper().Equals("FAILURE"))
                            {
                                iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                            }
                
                            DataRow NR = iPropertyTbl.NewRow();
                            NR.SetField<string>("PropertyID", detailProperty.SelectSingleNode("propertyId").InnerText);
                            NR.SetField<string>("MainAddr", detailProperty.SelectSingleNode("address").InnerText);
                            NR.SetField<Decimal>("CurrFees", currentFees);
                            NR.SetField<Decimal>("PriorFees", priorFees);
                            NR.SetField<Decimal>("CurrPenalty", currentPenalty);
                            NR.SetField<Decimal>("PriorPenalty", priorPenalty);
                            NR.SetField<Decimal>("Credits", credit);
                            NR.SetField<Decimal>("Balance", totalBalance);
                            iPropertyTbl.Rows.Add(NR);
                        }
                    }
                }

                gotUserProperties = iStatus.ToUpper().Equals("SUCCESS");

                if (!gotUserProperties)
                {
                    iStatus = "FAILURE";
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
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

            return gotUserProperties;
        }

        /// <summary>
        /// DONE, need mock data
        /// </summary>
        public static string GetPropertyUnits(string propertyID, string userCode, string billCode, string unitID = "")
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return "";
            }
            else
            {
                return GetPropertyUnits_Soap(propertyID, userCode, billCode, unitID);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static string GetPropertyUnits_Soap(string propertyID, string userCode, string billCode, string unitID = "")
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var result = "FAILURE";

            var tServices = "";
            var tUnitInfo = "";
            var tOccBy = "";
            var tExempt = "";
            var tStartDt = "";

            iStatus = "";

            if (iUnitsTbl.Columns.Count < 1)
            {
                iUnitsTbl.Columns.Add("chkUnit", typeof(bool));
                iUnitsTbl.Columns.Add("chkTenants", typeof(bool));
                iUnitsTbl.Columns.Add("UnitID", typeof(string));
                iUnitsTbl.Columns.Add("UnitNo", typeof(string));
                iUnitsTbl.Columns.Add("UnitStatID", typeof(string));
                iUnitsTbl.Columns.Add("UnitStatCode", typeof(string));
                iUnitsTbl.Columns.Add("CPUnitStatCode", typeof(string));
                iUnitsTbl.Columns.Add("CPUnitStatDisp", typeof(string));
                iUnitsTbl.Columns.Add("RentCeiling", typeof(Decimal));
                iUnitsTbl.Columns.Add("StartDt", typeof(DateTime));
                iUnitsTbl.Columns.Add("HServices", typeof(string));
            }

            // Clear out any units from prior call
            if (iUnitsTbl.Rows.Count > 0)
            {
                iUnitsTbl.Clear();
            }

            try
            {
                var xmlDoc = new XmlDocument();
                var soapMessage = NewSoapMessage();

                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:getPropertyAndUnitDetails>");
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", propertyID);
                soapMessage.Append("<request>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.Length == 0 ? "?" : userCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.Length == 0 ? "?" : billCode.EscapeXMLChars());
                soapMessage.Append("</request>");
                soapMessage.Append("</api:getPropertyAndUnitDetails>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
                    iStatus = "SUCCESS";
                    iBillAddr = "";
                    iAgentName = "";
                    iPropAddr = "";

                    if (detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress") != null)
                    {
                        iPropAddr = detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress") != null)
                    {
                        iBillAddr = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("agentDetails") != null)
                    {
                        iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText;
                        iAgentName = iAgentName.EscapeXMLChars();
                    }

                    tUnitInfo = "";
                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        Decimal rentCeiling = 0;

                        tServices = "";
                        tOccBy = "";
                        DataRow NR = iUnitsTbl.NewRow();
                        NR.SetField<bool>("chkUnit", false);
                        NR.SetField<bool>("chkTenants", false);

                        NR.SetField<string>("UnitID", detailUnits.SelectSingleNode("unitId").InnerText);
                        NR.SetField<string>("UnitNo", detailUnits.SelectSingleNode("unitNumber").InnerText);
                        NR.SetField<string>("UnitStatID", detailUnits.SelectSingleNode("unitStatusId").InnerText);
                        NR.SetField<string>("UnitStatCode", detailUnits.SelectSingleNode("unitStatusCode").InnerText);
                        NR.SetField<string>("CPUnitStatCode", detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText);

                        if ((detailUnits.SelectSingleNode("rentCeiling").InnerText.Length > 0))
                        {
                            Decimal.TryParse(detailUnits.SelectSingleNode("rentCeiling").InnerText, out rentCeiling);
                        }

                        NR.SetField<Decimal>("RentCeiling", rentCeiling);

                        if (detailUnits.SelectSingleNode("unitStatusAsOfDate") != null)
                        {
                            if (!string.IsNullOrEmpty(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText))
                            {
                                NR.SetField<DateTime>("StartDt", DateTime.Parse(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText));
                            }
                        }

                        foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
                        {
                            if (tServices.Length > 0)
                            {
                                tServices += (", " + detailService.SelectSingleNode("serviceName").InnerText);
                            }
                            else
                            {
                                tServices = detailService.SelectSingleNode("serviceName").InnerText;
                            }
                        }

                        NR.SetField<string>("HServices", tServices);

                        switch (NR.Field<string>("UnitStatCode").ToString().ToUpper())
                        {
                            case "OOCC":
                                NR.SetField<string>("CPUnitStatDisp", "Owner-Occupied");
                                break;
                            case "SEC8":
                                NR.SetField<string>("CPUnitStatDisp", "Section 8");
                                break;
                            case "RENTED":
                                NR.SetField<string>("CPUnitStatDisp", "Rented or Available for Rent");
                                break;
                            case "FREE":
                                NR.SetField<string>("CPUnitStatDisp", "Rent-Free");
                                break;
                            case "NAR":
                                NR.SetField<string>("CPUnitStatDisp", "Not Available for Rent");
                                break;
                            case "SPLUS":
                                NR.SetField<string>("CPUnitStatDisp", "Shelter Plus");
                                break;
                            case "DUPLEX":
                                NR.SetField<string>("CPUnitStatDisp", "Owner-occupied Duplex");
                                break;
                            case "COMM":
                                NR.SetField<string>("CPUnitStatDisp", "Commercial");
                                break;
                            case "SHARED":
                                NR.SetField<string>("CPUnitStatDisp", "Owner Shares Kit/Bath");
                                break;
                            case "MISC":
                                NR.SetField<string>("CPUnitStatDisp", "Miscellaneous Exempt");
                                break;
                        }

                        iUnitsTbl.Rows.Add(NR);

                        // If a unit was passed in fill out information if this is the correct unit
                        if (unitID.Length > 0 && unitID == NR.Field<string>("UnitID"))
                        {
                            tExempt = "";
                            tStartDt = "";

                            if (!string.IsNullOrEmpty(NR.Field<DateTime>("StartDt").ToString()) && NR.Field<DateTime>("StartDt").ToString().Length > 0)
                            {
                                tStartDt = DateTime.Parse(NR.Field<DateTime>("StartDt").ToString()).ToString("MM/dd/yyyy");
                            }

                            foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                            {
                                if ((tOccBy.Length > 0))
                                {
                                    tOccBy += (", " + detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText);
                                }
                                else
                                {
                                    tOccBy = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
                                }

                            }

                            tOccBy = tOccBy.UnescapeXMLChars();

                            switch (NR.Field<string>("UnitStatCode").ToUpper())
                            {
                                case "OOCC":
                                    tExempt = "Owner-Occupied";
                                    break;
                                case "SEC8":
                                    tExempt = "Section 8";
                                    break;
                                case "RENTED":
                                    tExempt = "Rented or Available for Rent";
                                    break;
                                case "FREE":
                                    tExempt = "Rent-Free";
                                    break;
                                case "NAR":
                                    tExempt = "Not Available for Rent";
                                    break;
                                case "SPLUS":
                                    tExempt = "Shelter Plus";
                                    break;
                                case "DUPLEX":
                                    tExempt = "Owner-occupied Duplex";
                                    break;
                                case "COMM":
                                    tExempt = "Commercial";
                                    break;
                                case "SHARED":
                                    tExempt = "Owner Shares Kit/Bath";
                                    break;
                                case "MISC":
                                    tExempt = "Miscellaneous Exempt";
                                    break;
                            }

                            var fields = new Dictionary<string, string>();

                            fields.Add("CPStatus", NR.Field<string>("CPUnitStatCode"));
                            fields.Add("ExReason", tExempt);
                            fields.Add("StartDt", tStartDt);
                            fields.Add("OccBy", tOccBy);
                            fields.Add("UnitID", NR.Field<string>("UnitID"));

                            tUnitInfo = fields.ToDelimitedString();
                            //tUnitInfo = ("CPStatus="
                            //            + (NR.Item["CPUnitStatCode"].ToString + ("::ExReason="
                            //            + (tExempt + ("::StartDt="
                            //            + (tStartDt + ("::OccBy="
                            //            + (tOccBy + ("::UnitID=" + NR.Item["UnitID"].ToString)))))))));
                        }
                    }
                }

                if (iStatus.ToUpper() != "SUCCESS")
                {
                    iStatus = "FAILURE";
                }

                if (tUnitInfo.Length > 0)
                {
                    result = tUnitInfo;
                }
                else
                {
                    result = iStatus.ToUpper();
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
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

            return result;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static string GetPropertyTenants(string propertyID, string userCode, string billCode, string unitID)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return ""; // TODO: get mock data
            }
            else
            {
                return GetPropertyTenants_Soap(propertyID, userCode, billCode, unitID);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static string GetPropertyTenants_Soap(string propertyID, string userCode, string billCode, string unitID)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var result = "FAILURE";

            iStatus = "";
            var tUnitInfo = "";

            if (iTenantsTbl.Columns.Count < 1)
            {
                iTenantsTbl.Columns.Add("TenantID", typeof(string));
                iTenantsTbl.Columns.Add("FirstName", typeof(string));
                iTenantsTbl.Columns.Add("LastName", typeof(string));
                iTenantsTbl.Columns.Add("DispName", typeof(string));
                iTenantsTbl.Columns.Add("PhoneNo", typeof(string));
                iTenantsTbl.Columns.Add("EmailAddr", typeof(string));
            }

            // Clear out any units from prior call
            if (iTenantsTbl.Rows.Count > 0)
            {
                iTenantsTbl.Clear();
            }

            try
            {
                var xmlDoc = new XmlDocument();
                var soapMessage = NewSoapMessage();

                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:getPropertyAndUnitDetails>");
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", propertyID);
                soapMessage.Append("<request>");
                soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userCode.Length == 0 ? "?" : userCode.EscapeXMLChars());
                soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", billCode.Length == 0 ? "?" : billCode.EscapeXMLChars());
                soapMessage.Append("</request>");
                soapMessage.Append("</api:getPropertyAndUnitDetails>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
                    iStatus = "SUCCESS";
                    iBillAddr = "";
                    iAgentName = "";
                    iPropAddr = "";
                    iBillContact = "";
                    iBillEmail = "";

                    var tServices = "";
                    DateTime tStartDt;
                    string tPriorDt = "";
                    var tPriorReas = "";
                    var tSmokYN = "";
                    var tSmokDt = "";
                    var tInitRent = "";
                    int TenCnt = 0;

                    if (detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress") != null)
                    {
                        iPropAddr = detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress") != null)
                    {
                        iBillAddr = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay") != null)
                    {
                        iBillContact = detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay").InnerText;
                        iBillContact = iBillContact.UnescapeXMLChars();
                    }

                    if (detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress") != null)
                    {
                        iBillEmail = detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress").InnerText;
                    }

                    if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName") != null)
                    {
                        iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText;
                        iAgentName = iAgentName.UnescapeXMLChars();
                    }

                    if (iAgentName.Length < 1)
                    {
                        if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName") != null)
                        {
                            iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName").InnerText;
                            iAgentName = iAgentName.UnescapeXMLChars();
                        }
                    }

                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        tServices = "";
                        if (unitID == detailUnits.SelectSingleNode("unitId").InnerText)
                        {
                            tStartDt = DateTime.MinValue;
                            if (detailUnits.SelectSingleNode("tenancyStartDate") != null)
                            {
                                if (!string.IsNullOrEmpty(detailUnits.SelectSingleNode("tenancyStartDate").InnerText))
                                {
                                    tStartDt = DateTime.Parse(detailUnits.SelectSingleNode("tenancyStartDate").InnerText);
                                }

                            }

                            foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
                            {
                                if ((tServices.Length > 0))
                                {
                                    tServices += (", " + detailService.SelectSingleNode("serviceName").InnerText);
                                }
                                else
                                {
                                    tServices = detailService.SelectSingleNode("serviceName").InnerText;
                                }

                            }

                            TenCnt = 0;
                            if (detailUnits.GetElementsByTagName("noOfOccupants").Item(0) != null)
                            {
                                TenCnt = int.Parse(detailUnits.GetElementsByTagName("noOfOccupants").Item(0).InnerText);
                            }

                            tInitRent = "0.00";
                            if (detailUnits.GetElementsByTagName("initialRent").Item(0) != null)
                            {
                                tInitRent = detailUnits.GetElementsByTagName("initialRent").Item(0).InnerText;
                            }

                            tPriorDt = "";
                            if (detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0) != null)
                            {
                                tPriorDt = detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0).InnerText;
                            }

                            tPriorReas = "";
                            if (detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0) != null)
                            {
                                tPriorReas = detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0).InnerText;
                            }

                            tSmokYN = "";
                            if (detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0) != null)
                            {
                                tSmokYN = detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0).InnerText;
                            }

                            tSmokDt = "";
                            if (detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0) != null)
                            {
                                tSmokDt = detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0).InnerText;
                            }

                            foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                            {
                                var row = iTenantsTbl.NewRow();

                                row.SetField<string>("TenantID", detailOccBy.SelectSingleNode("occupantId").InnerText);
                                row.SetField<string>("FirstName", detailOccBy.SelectSingleNode("name").SelectSingleNode("firstName").InnerText);
                                row.SetField<string>("LastName", detailOccBy.SelectSingleNode("name").SelectSingleNode("lastName").InnerText);
                                row.SetField<string>("DispName", detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText);
                                row.SetField<string>("PhoneNo", detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("phoneNumber").InnerText);
                                row.SetField<string>("EmailAddr", detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("emailAddress").InnerText);

                                iTenantsTbl.Rows.Add(row);
                            }

                            var fields = new Dictionary<string, string>();

                            fields.Add("CPStatus", detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText);
                            fields.Add("HServices", tServices);
                            fields.Add("StartDt", tStartDt.ToString());
                            fields.Add("NumTenants", TenCnt.ToString());
                            fields.Add("SmokeYN", tSmokYN);
                            fields.Add("SmokeDt", tSmokDt);
                            fields.Add("InitRent", tInitRent);
                            fields.Add("PriorEndDt", tPriorDt);
                            fields.Add("TermReason", tPriorReas);
                            fields.Add("OwnerName", iBillContact);
                            fields.Add("AgenntName", iAgentName);
                            fields.Add("UnitID", detailUnits.SelectSingleNode("unitId").InnerText);
                            fields.Add("OwnerEmail", iBillEmail);

                            tUnitInfo = fields.ToDelimitedString();
                        }

                    }

                }

                if (iStatus.ToUpper() != "SUCCESS")
                {
                    iStatus = "FAILURE";
                }

                if (tUnitInfo.Length > 0)
                {
                    result = tUnitInfo;
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
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

            return iStatus;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool SaveCart(string soapString)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return true;
            }
            else
            {
                return SaveCart_Soap(soapString);
            }
        }

        /// <summary>
        /// DONE, need to update EditCart.aspx.cs
        /// </summary>
        public static bool SaveCart_Soap(string soapString)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var wasSaved = false;

            try
            {
                var xmlDoc = new XmlDocument();
                var fields = Parse(soapString);

                var userCode = fields.GetStringValue("UserID");
                var billCode = fields.GetStringValue("BillingCode");

                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:savePaymentCart>");
                soapMessage.Append("<savePaymentToCart>");
                soapMessage.AppendFormat("<cartId>{0}</cartId>", "tbd");
                soapMessage.Append("<paymentConfirmationNo/>");
                soapMessage.Append("<paymentReceivedAmt/>");
                soapMessage.AppendFormat("<isFeeOnlyPaid>{0}</isFeeOnlyPaid>", "tbd");

                // for each item
                soapMessage.Append("<items>");
                soapMessage.AppendFormat("<itemId>{0}</itemId>", "tbd");
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", "tbd");
                soapMessage.AppendFormat("<propertyMainStreetAddress>{0}</propertyMainStreetAddress>", "tbd");
                soapMessage.AppendFormat("<fee>{0}</fee>", "tbd");
                soapMessage.AppendFormat("<penalties></penalties>", "tbd");
                soapMessage.AppendFormat("<balance></balance>", "tbd");
                soapMessage.Append("</items");

                soapMessage.Append("</savePaymentToCart>");
                soapMessage.Append("</api:savePaymentCart>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "SavePaymentCartDetails/RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Binder_savePaymentCart");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

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
                iErrMsg = ex.ToString();
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

            return wasSaved;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool SaveUnit(string soapString)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return true;
            }
            else
            {
                return SaveUnit_Soap(soapString);
            }
        }

        /// <summary>
        /// DONE, need to update Properties/UpdateUnit.aspx.cs
        /// </summary>
        public static bool SaveUnit_Soap(string soapString)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var wasSaved = false;

            try
            {
                var xmlDoc = new XmlDocument();
                var fields = Parse(soapString);

                var userCode = fields.GetStringValue("UserID");
                var billCode = fields.GetStringValue("BillingCode");

                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:updateUnitStatusChange>");
                soapMessage.Append("<unitStatusChangeReq>");
                soapMessage.AppendFormat("<userId>{0}</userId>", "from session");
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", "from session");
                soapMessage.AppendFormat("<unitId>{0}</unitId>", "from form");
                soapMessage.AppendFormat("<clientPortalUnitStatusCode>{0}</clientPortalUnitStatusCode>", "from form");
                soapMessage.AppendFormat("<unitStatus>{0}</unitStatus>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><exemptionReason>{0}</exemptionReason>", "from form");
                soapMessage.AppendFormat("<unitStatusAsOfDate>{0}</unitStatusAsOfDate>", "from form");
                soapMessage.AppendFormat("<declarationInitial>{0}</declarationInitial>", "from form");
                soapMessage.Append("<questions>");
                soapMessage.AppendFormat("<!--Optional:--><asOfDate>{0}</asOfDate>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><dateStarted>{0}</dateStarted>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><occupiedBy>{0}</occupiedBy>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><contractNo>{0}</contractNo>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><commeUseDesc>{0}</commeUseDesc>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><isCommeUseZoned>{0}</isCommeUseZoned>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><isExclusivelyForCommeUse>{0}</isExclusivelyForCommeUse>", "from form");
               
                // Next 3 removed  when Owner Occupied Exempt Duplex was removed from the Other dropdown
                soapMessage.Append("<!--Optional:--><_x0035_0PercentAsOf31Dec1979></_x0035_0PercentAsOf31Dec1979>");
                soapMessage.Append("<!--Optional:--><ownerOccupantName></ownerOccupantName>");
                soapMessage.Append("<!--Zero or more repetitions:--><namesOfownersOfRecord></namesOfownersOfRecord>");

                soapMessage.AppendFormat("<!--Optional:--><nameOfPropertyManagerResiding>{0}</nameOfPropertyManagerResiding>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><emailOfPhoneOfPropertyManagerResiding>{0}</emailOfPhoneOfPropertyManagerResiding>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><IsOwnersPrinciplePlaceOfResidence>{0}</IsOwnersPrinciplePlaceOfResidence>", "from form");
                soapMessage.AppendFormat("<!--Optional:--><doesOwnerResideInOtherUnitOfThisUnitProperty>{0}</doesOwnerResideInOtherUnitOfThisUnitProperty>", "from form");
                soapMessage.Append("<!--Zero or more repetitions:--><tenantsAndContactInfo>");
                soapMessage.AppendFormat("<name>{0}</name>", "from form");
                soapMessage.AppendFormat("<contactInfo>{0}</contactInfo>", "from form");
                soapMessage.Append("</tenantsAndContactInfo>");
                soapMessage.Append("<questions>");
                soapMessage.Append("</unitStatusChangeReq>");
                soapMessage.Append("</api:updateUnitStatusChange>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "UpdateUnitTenancy/RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Binder_updateUnitTenancy");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

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
                iErrMsg = ex.ToString();
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

            return wasSaved;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool SaveTenant(string soapString)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                return true;
            }
            else
            {
                return SaveTenant_Soap(soapString);
            }
        }

        /// <summary>
        /// DONE, need to update Properties/UpdateTenancy.aspx.cs
        /// </summary>
        public static bool SaveTenant_Soap(string soapString)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var wasSaved = false;

            try
            {
                var xmlDoc = new XmlDocument();
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

                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "UpdateUnitTenancy/RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Binder_updateUnitTenancy");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                xmlDoc.LoadXml(reader.ReadToEnd());

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
                iErrMsg = ex.ToString();
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

            return wasSaved;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ValidateReset(UserProfile userProfile)
        {
            iErrMsg = "";

            if (USE_MOCK_SERVICES)
            {
                // TODO: need mock object
                return true;
            }
            else
            {
                return ValidateReset_Soap(userProfile);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ValidateReset_Soap(UserProfile userProfile)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var canReset = false;

            var soapMessage = NewSoapMessage();
            soapMessage.Append("<soapenv:Header/>");
            soapMessage.Append("<soapenv:Body>");
            soapMessage.Append("<api:validateResetUserPassword>");
            soapMessage.Append("<resetUserPwdReq>");
            soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userProfile.UserCode.EscapeXMLChars());
            soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", userProfile.BillingCode.Length == 0 ? "?" : userProfile.BillingCode.EscapeXMLChars());
            soapMessage.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", userProfile.Question1.EscapeXMLChars());
            soapMessage.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", userProfile.Answer1.EscapeXMLChars());
            soapMessage.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", userProfile.Question2.EscapeXMLChars());
            soapMessage.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", userProfile.Answer2.EscapeXMLChars());
            soapMessage.Append("</resetUserPwdReq>");
            soapMessage.Append("</api:validateResetUserPassword>");
            soapMessage.Append("</soapenv:Body>");
            soapMessage.Append("</soapenv:Envelope>");

            try
            {
                var doc = new XmlDocument();
                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                request = WebRequest.Create(uriPrefix + "ValidateResetPasswordRequest/RTSClientPortalAPI_API_WSD_ValidateResetPasswordRequest_Port");
                request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_ValidateResetPasswordRequest_Binder_validateResetUserPassword");
                request.ContentType = "text/xml; charset=utf-8";
                request.ContentLength = soapByte.Length;
                request.Method = "POST";

                requestStream = request.GetRequestStream();
                requestStream.Write(soapByte, 0, soapByte.Length);

                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                doc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("response"))
                {
                    iStatus = detail.ChildNodes[0].InnerText;
                    if (iStatus.ToUpper() != "SUCCESS")
                    {
                        iErrMsg = detail.ChildNodes[1].InnerText;
                    }

                }

                canReset =  iStatus.ToUpper().Equals("SUCCESS");
            }
            catch (WebException ex)
            {
                iErrMsg = ex.ToString();            }
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

            return canReset;
        }

        public static bool CheckPswdRules(string aPwd, string aUserID)
        {
            bool RetValue = false;
            bool tResult = true;
            bool testB;
            Regex RegexObj = new Regex("[a-zA-Z0-9!@#$%^&_*]");

            // Test Length
            if ((aPwd.Length < 7 || aPwd.Length > 20))
            {
                tResult = false;
            }

            testB = false;
            if (RegexObj.IsMatch(aPwd))
            {
                testB = true;
            }

            if ((testB == false))
            {
                tResult = false;
            }

            testB = false;
            if (aPwd.IndexOf(aUserID) > -1)
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
