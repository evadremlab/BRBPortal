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
            if (USE_MOCK_SERVICES)
            {
                iRelate = "";
                iErrMsg = "";
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
            if (USE_MOCK_SERVICES)
            {
                iErrMsg = "";
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

                // clear session vars
                iStatus = "";

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
        public static bool UpdateProfile(string soapString)
        {
            if (USE_MOCK_SERVICES)
            {
                return true;
            }
            else
            {
                return UpdateProfile_Soap(soapString);
            }
        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool UpdateProfile_Soap(string soapString)
        {
            // SEE Test/Live Data/GetProfile_Response.txt for sample data

            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var wasUpdated = false;

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

                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:updateUserProfile>");
                soapMessage.Append("<updateUserProfileReq>");
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
                soapMessage.Append("</updateUserProfileReq>");
                soapMessage.Append("<isActive>Y</isActive>");
                soapMessage.Append("</api:updateUserProfile>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var xmlDoc = new XmlDocument();
                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                iStatus = "";

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
        /// DONE
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

        public static bool GetUserProperties_Soap(string aID, string aBillCd)
        {
            // TODO: uncomment and convert
            return true;

            //WebRequest Request;
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //string SoapStr;
            //iStatus = "";

            //if ((iPropertyTbl.Columns.Count < 1))
            //{
            //    iPropertyTbl.Columns.Add("chkProp", typeof(string));
            //    iPropertyTbl.Columns.Add("PropertyID", typeof(string));
            //    iPropertyTbl.Columns.Add("MainAddr", typeof(string));
            //    iPropertyTbl.Columns.Add("CurrFees", typeof(Decimal));
            //    iPropertyTbl.Columns.Add("PriorFees", typeof(Decimal));
            //    iPropertyTbl.Columns.Add("CurrPenalty", typeof(Decimal));
            //    iPropertyTbl.Columns.Add("PriorPenalty", typeof(Decimal));
            //    iPropertyTbl.Columns.Add("Credits", typeof(Decimal));
            //    iPropertyTbl.Columns.Add("Balance", typeof(Decimal));
            //    iPropertyTbl.Columns.Add("btnUpdProp", typeof(string));
            //}

            //// Clear out any properties from prior call
            //if ((iPropertyTbl.Rows.Count > 0))
            //{
            //    iPropertyTbl.Clear();
            //}

            //SoapStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:api=\"http://cityofb" +
            //"erkeley.info/RTS/ClientPortal/API\">";
            //SoapStr += "<soapenv:Header/>";
            //SoapStr += "<soapenv:Body>";
            //SoapStr += "<api:getProfileProperties>";
            //SoapStr += "<request>";
            //if ((aID.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><userId>" + (aID + "</userId>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><userId>?</userId>";
            //}

            //if ((aBillCd.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><billingCode>" + (aBillCd + "</billingCode>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><billingCode>?</billingCode>";
            //}

            //SoapStr += "</request>";
            //SoapStr += "</api:getProfileProperties>";
            //SoapStr += "</soapenv:Body>";
            //SoapStr += "</soapenv:Envelope>";

            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.GetUserProfilePropertiesList/RTSCl" +
            //        "ientPortalAPI_API_WSD_GetUserProfilePropertiesList_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Binder_getProfileProperties");
            //    Request.ContentType = "text/xml; charset=utf-8";
            //    Request.ContentLength = SoapByte.Length;
            //    Request.Method = "POST";
            //    DataStream = Request.GetRequestStream();
            //    DataStream.Write(SoapByte, 0, SoapByte.Length);
            //    DataStream.Close();
            //    Response = Request.GetResponse();
            //    DataStream = Response.GetResponseStream();
            //    Reader = new StreamReader(DataStream);
            //    string SD2Request = Reader.ReadToEnd();
            //    DataStream.Close();
            //    Reader.Close();
            //    Response.Close();

            //    // Set session variables from response
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(SD2Request);

            //    foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("response"))
            //    {
            //        foreach (XmlElement detailProperty in detail.GetElementsByTagName("profileProperty"))
            //        {
            //            foreach (XmlElement detailAmounts in detailProperty.GetElementsByTagName("balanceAmounts"))
            //            {
            //                iStatus = detail.SelectSingleNode("status").InnerText;
            //                if ((iStatus.ToUpper == "FAILURE"))
            //                {
            //                    iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
            //                }

            //                DataRow NR = iPropertyTbl.NewRow();
            //                NR.Item["PropertyID"] = detailProperty.SelectSingleNode("propertyId").InnerText;
            //                NR.Item["MainAddr"] = detailProperty.SelectSingleNode("address").InnerText;
            //                NR.Item["CurrFees"] = detailAmounts.SelectSingleNode("currentFees").InnerText;
            //                NR.Item["PriorFees"] = detailAmounts.SelectSingleNode("priorFees").InnerText;
            //                NR.Item["CurrPenalty"] = detailAmounts.SelectSingleNode("currentPenalty").InnerText;
            //                NR.Item["PriorPenalty"] = detailAmounts.SelectSingleNode("priorPenalties").InnerText;
            //                NR.Item["Credits"] = detailAmounts.SelectSingleNode("credit").InnerText;
            //                NR.Item["Balance"] = detailAmounts.SelectSingleNode("totalBalance").InnerText;
            //                iPropertyTbl.Rows.Add(NR);
            //            }
            //        }
            //    }

            //    if ((iStatus.ToUpper == "SUCCESS"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        iStatus = "FAILURE";
            //        return false;
            //    }
            //}
            //catch (WebException ex)
            //{
            //    iErrMsg = ex.ToString;
            //    // MsgBox(ex.ToString())
            //    return false;
            //}
        }

        public static string GetPropertyUnits_Soap(string aPropID, string aID, string aBillCd, string aUnitID = "")
        {
            // TODO: uncomment and convert
            return "TODO";

            //WebRequest Request;
            //// Warning!!! Optional parameters not supported
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //string SoapStr;
            //string tServices;
            //string tUnitInfo;
            //string tOccBy;
            //string tExempt;
            //string tStartDt;
            //iStatus = "";
            //tUnitInfo = "";

            //if ((iUnitsTbl.Columns.Count < 1))
            //{
            //    iUnitsTbl.Columns.Add("chkUnit", typeof(bool));
            //    iUnitsTbl.Columns.Add("chkTenants", typeof(bool));
            //    iUnitsTbl.Columns.Add("UnitID", typeof(string));
            //    iUnitsTbl.Columns.Add("UnitNo", typeof(string));
            //    iUnitsTbl.Columns.Add("UnitStatID", typeof(string));
            //    iUnitsTbl.Columns.Add("UnitStatCode", typeof(string));
            //    iUnitsTbl.Columns.Add("CPUnitStatCode", typeof(string));
            //    iUnitsTbl.Columns.Add("CPUnitStatDisp", typeof(string));
            //    iUnitsTbl.Columns.Add("RentCeiling", typeof(Decimal));
            //    iUnitsTbl.Columns.Add("StartDt", typeof(DateTime));
            //    iUnitsTbl.Columns.Add("HServices", typeof(string));
            //}

            //// Clear out any units from prior call
            //if ((iUnitsTbl.Rows.Count > 0))
            //{
            //    iUnitsTbl.Clear();
            //}

            //SoapStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:api=\"http://cityofb" +
            //"erkeley.info/RTS/ClientPortal/API\">";
            //SoapStr += "<soapenv:Header/>";
            //SoapStr += "<soapenv:Body>";
            //SoapStr += "<api:getPropertyAndUnitDetails>";
            //SoapStr = (SoapStr + ("<propertyId>" + (aPropID + "</propertyId>")));
            //SoapStr += "<request>";
            //if ((aID.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><userId>" + (aID + "</userId>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><userId>?</userId>";
            //}

            //if ((aBillCd.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><billingCode>" + (aBillCd + "</billingCode>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><billingCode>?</billingCode>";
            //}

            //SoapStr += "</request>";
            //SoapStr += "</api:getPropertyAndUnitDetails>";
            //SoapStr += "</soapenv:Body>";
            //SoapStr += "</soapenv:Envelope>";

            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.GetPropertyAndUnitDetails/RTSClien" +
            //        "tPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails");
            //    Request.ContentType = "text/xml; charset=utf-8";
            //    Request.ContentLength = SoapByte.Length;
            //    Request.Method = "POST";
            //    DataStream = Request.GetRequestStream();
            //    DataStream.Write(SoapByte, 0, SoapByte.Length);
            //    DataStream.Close();
            //    Response = Request.GetResponse();
            //    DataStream = Response.GetResponseStream();
            //    Reader = new StreamReader(DataStream);
            //    string SD2Request = Reader.ReadToEnd();
            //    DataStream.Close();
            //    Reader.Close();
            //    Response.Close();

            //    // Set session variables from response
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(SD2Request);

            //    foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
            //    {
            //        iStatus = "SUCCESS";
            //        iBillAddr = "";
            //        iAgentName = "";
            //        iPropAddr = "";

            //        if (detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress") != null)
            //        {
            //            iPropAddr = detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText;
            //        }

            //        if (detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress") != null)
            //        {
            //            iBillAddr = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText;
            //        }

            //        if (detail.SelectSingleNode("agentDetails") != null)
            //        {
            //            iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText;
            //            iAgentName = EscapeXMLChars(iAgentName);
            //        }

            //        tUnitInfo = "";
            //        foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
            //        {
            //            tServices = "";
            //            tOccBy = "";
            //            DataRow NR = iUnitsTbl.NewRow();
            //            NR.Item["chkUnit"] = false;
            //            NR.Item["chkTenants"] = false;
            //            NR.Item["UnitID"] = detailUnits.SelectSingleNode("unitId").InnerText;
            //            NR.Item["UnitNo"] = detailUnits.SelectSingleNode("unitNumber").InnerText;
            //            NR.Item["UnitStatID"] = detailUnits.SelectSingleNode("unitStatusId").InnerText;
            //            NR.Item["UnitStatCode"] = detailUnits.SelectSingleNode("unitStatusCode").InnerText;
            //            NR.Item["CPUnitStatCode"] = detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText;
            //            if ((detailUnits.SelectSingleNode("rentCeiling").InnerText.Length > 0))
            //            {
            //                NR.Item["RentCeiling"] = detailUnits.SelectSingleNode("rentCeiling").InnerText;
            //            }
            //            else
            //            {
            //                NR.Item["RentCeiling"] = ((Decimal)(0));
            //            }

            //            if (detailUnits.SelectSingleNode("unitStatusAsOfDate") != null)
            //            {
            //                if (!IsDBNull(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText))
            //                {
            //                    NR.Item["StartDt"] = DateTime.Parse(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText);
            //                }
            //            }

            //            foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
            //            {
            //                if ((tServices.Length > 0))
            //                {
            //                    tServices = (tServices + (", " + detailService.SelectSingleNode("serviceName").InnerText));
            //                }
            //                else
            //                {
            //                    tServices = detailService.SelectSingleNode("serviceName").InnerText;
            //                }
            //            }

            //            NR.Item["HServices"] = tServices;
            //            switch (NR.Item["UnitStatCode"].ToString.ToUpper)
            //            {
            //                case "OOCC":
            //                    NR.Item["CPUnitStatDisp"] = "Owner-Occupied";
            //                    break;
            //                case "SEC8":
            //                    NR.Item["CPUnitStatDisp"] = "Section 8";
            //                    break;
            //                case "RENTED":
            //                    NR.Item["CPUnitStatDisp"] = "Rented or Available for Rent";
            //                    break;
            //                case "FREE":
            //                    NR.Item["CPUnitStatDisp"] = "Rent-Free";
            //                    break;
            //                case "NAR":
            //                    NR.Item["CPUnitStatDisp"] = "Not Available for Rent";
            //                    break;
            //                case "SPLUS":
            //                    NR.Item["CPUnitStatDisp"] = "Shelter Plus";
            //                    break;
            //                case "DUPLEX":
            //                    NR.Item["CPUnitStatDisp"] = "Owner-occupied Duplex";
            //                    break;
            //                case "COMM":
            //                    NR.Item["CPUnitStatDisp"] = "Commercial";
            //                    break;
            //                case "SHARED":
            //                    NR.Item["CPUnitStatDisp"] = "Owner Shares Kit/Bath";
            //                    break;
            //                case "MISC":
            //                    NR.Item["CPUnitStatDisp"] = "Miscellaneous Exempt";
            //                    break;
            //            }

            //            iUnitsTbl.Rows.Add(NR);
            //            // If a unit was passed in fill out information if this is the correct unit
            //            if (((aUnitID.Length > 0) && (aUnitID == NR.Item["UnitID"].ToString)))
            //            {
            //                tExempt = "";
            //                tStartDt = "";
            //                if (!string.IsNullOrEmpty(NR.Item("StartDt")) && NR.Item("StartDt").ToString.Length > 0) {
            //                    tStartDt = DateTime.Parse(NR.Item["StartDt"].ToString).ToString("MM/dd/yyyy");
            //                }

            //                foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
            //                {
            //                    if ((tOccBy.Length > 0))
            //                    {
            //                        tOccBy = (tOccBy + (", " + detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText));
            //                    }
            //                    else
            //                    {
            //                        tOccBy = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
            //                    }

            //                }

            //                tOccBy = UnescapeXMLChars(tOccBy);
            //                switch (NR.Item["UnitStatCode"].ToString.ToUpper)
            //                {
            //                    case "OOCC":
            //                        tExempt = "Owner-Occupied";
            //                        break;
            //                    case "SEC8":
            //                        tExempt = "Section 8";
            //                        break;
            //                    case "RENTED":
            //                        tExempt = "Rented or Available for Rent";
            //                        break;
            //                    case "FREE":
            //                        tExempt = "Rent-Free";
            //                        break;
            //                    case "NAR":
            //                        tExempt = "Not Available for Rent";
            //                        break;
            //                    case "SPLUS":
            //                        tExempt = "Shelter Plus";
            //                        break;
            //                    case "DUPLEX":
            //                        tExempt = "Owner-occupied Duplex";
            //                        break;
            //                    case "COMM":
            //                        tExempt = "Commercial";
            //                        break;
            //                    case "SHARED":
            //                        tExempt = "Owner Shares Kit/Bath";
            //                        break;
            //                    case "MISC":
            //                        tExempt = "Miscellaneous Exempt";
            //                        break;
            //                }
            //                tUnitInfo = ("CPStatus="
            //                            + (NR.Item["CPUnitStatCode"].ToString + ("::ExReason="
            //                            + (tExempt + ("::StartDt="
            //                            + (tStartDt + ("::OccBy="
            //                            + (tOccBy + ("::UnitID=" + NR.Item["UnitID"].ToString)))))))));
            //            }

            //        }

            //    }

            //    if ((iStatus.ToUpper != "SUCCESS"))
            //    {
            //        iStatus = "FAILURE";
            //    }

            //    if ((tUnitInfo.Length > 0))
            //    {
            //        return tUnitInfo;
            //    }

            //    return iStatus;
            //}
            //catch (WebException ex)
            //{
            //    iErrMsg = ex.ToString;
            //    // MsgBox(ex.ToString())
            //    return "FAILURE";
            //}

        }

        public static string GetPropertyTenants_Soap(string aPropID, string aID, string aBillCd, string aUnitID)
        {
            // TODO: uncomment and convert
            return "TODO";

            //WebRequest Request;
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //string SoapStr;
            //string tServices;
            //string tUnitInfo;
            //string tExempt;
            //string tStartDt;
            //string tPriorDt;
            //string tPriorReas;
            //string tSmokYN;
            //string tSmokDt;
            //string tInitRent;
            //int TenCnt;
            //iStatus = "";
            //tUnitInfo = "";

            //if ((iTenantsTbl.Columns.Count < 1))
            //{
            //    iTenantsTbl.Columns.Add("TenantID", typeof(string));
            //    iTenantsTbl.Columns.Add("FirstName", typeof(string));
            //    iTenantsTbl.Columns.Add("LastName", typeof(string));
            //    iTenantsTbl.Columns.Add("DispName", typeof(string));
            //    iTenantsTbl.Columns.Add("PhoneNo", typeof(string));
            //    iTenantsTbl.Columns.Add("EmailAddr", typeof(string));
            //}

            //// Clear out any units from prior call
            //if ((iTenantsTbl.Rows.Count > 0))
            //{
            //    iTenantsTbl.Clear();
            //}

            //SoapStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:api=\"http://cityofb" +
            //"erkeley.info/RTS/ClientPortal/API\">";
            //SoapStr += "<soapenv:Header/>";
            //SoapStr += "<soapenv:Body>";
            //SoapStr += "<api:getPropertyAndUnitDetails>";
            //SoapStr = (SoapStr + ("<propertyId>" + (aPropID + "</propertyId>")));
            //SoapStr += "<request>";
            //if ((aID.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><userId>" + (aID + "</userId>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><userId>?</userId>";
            //}

            //if ((aBillCd.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><billingCode>" + (aBillCd + "</billingCode>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><billingCode>?</billingCode>";
            //}

            //SoapStr += "</request>";
            //SoapStr += "</api:getPropertyAndUnitDetails>";
            //SoapStr += "</soapenv:Body>";
            //SoapStr += "</soapenv:Envelope>";
            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.GetPropertyAndUnitDetails/RTSClien" +
            //        "tPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails");
            //    Request.ContentType = "text/xml; charset=utf-8";
            //    Request.ContentLength = SoapByte.Length;
            //    Request.Method = "POST";
            //    DataStream = Request.GetRequestStream();
            //    DataStream.Write(SoapByte, 0, SoapByte.Length);
            //    DataStream.Close();
            //    Response = Request.GetResponse();
            //    DataStream = Response.GetResponseStream();
            //    Reader = new StreamReader(DataStream);
            //    string SD2Request = Reader.ReadToEnd();
            //    DataStream.Close();
            //    Reader.Close();
            //    Response.Close();
            //    // Set session variables from response
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(SD2Request);
            //    foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
            //    {
            //        iStatus = "SUCCESS";
            //        iBillAddr = "";
            //        iAgentName = "";
            //        iPropAddr = "";
            //        iBillContact = "";
            //        iBillEmail = "";
            //        if (detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress"))
            //        {
            //            IsNot;
            //            null;
            //            iPropAddr = detail.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText;
            //        }

            //        if (detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress"))
            //        {
            //            IsNot;
            //            null;
            //            iBillAddr = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText;
            //        }

            //        if (detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay"))
            //        {
            //            IsNot;
            //            null;
            //            iBillContact = detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay").InnerText;
            //            iBillContact = UnescapeXMLChars(iBillContact);
            //        }

            //        if (detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress"))
            //        {
            //            IsNot;
            //            null;
            //            iBillEmail = detail.SelectSingleNode("billingDetails").SelectSingleNode("contact").SelectSingleNode("emailAddress").InnerText;
            //        }

            //        if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName"))
            //        {
            //            IsNot;
            //            null;
            //            iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText;
            //            iAgentName = UnescapeXMLChars(iAgentName);
            //        }

            //        if ((iAgentName.Length < 1))
            //        {
            //            if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName"))
            //            {
            //                IsNot;
            //                null;
            //                iAgentName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName").InnerText;
            //                iAgentName = UnescapeXMLChars(iAgentName);
            //            }

            //        }

            //        tUnitInfo = "";
            //        TenCnt = 0;
            //        foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
            //        {
            //            tServices = "";
            //            if ((aUnitID == detailUnits.SelectSingleNode("unitId").InnerText))
            //            {
            //                tExempt = "";
            //                tStartDt = "";
            //                if (detailUnits.SelectSingleNode("tenancyStartDate"))
            //                {
            //                    IsNot;
            //                    null;
            //                    if (!IsDBNull(detailUnits.SelectSingleNode("tenancyStartDate").InnerText))
            //                    {
            //                        tStartDt = DateTime.Parse(detailUnits.SelectSingleNode("tenancyStartDate").InnerText);
            //                    }

            //                }

            //                foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
            //                {
            //                    if ((tServices.Length > 0))
            //                    {
            //                        tServices = (tServices + (", " + detailService.SelectSingleNode("serviceName").InnerText));
            //                    }
            //                    else
            //                    {
            //                        tServices = detailService.SelectSingleNode("serviceName").InnerText;
            //                    }

            //                }

            //                TenCnt = 0;
            //                if (detailUnits.GetElementsByTagName("noOfOccupants").Item[0])
            //                {
            //                    IsNot;
            //                    null;
            //                    TenCnt = int.Parse(detailUnits.GetElementsByTagName("noOfOccupants").Item[0].InnerText);
            //                }

            //                tInitRent = "0.00";
            //                if (detailUnits.GetElementsByTagName("initialRent").Item[0])
            //                {
            //                    IsNot;
            //                    null;
            //                    tInitRent = detailUnits.GetElementsByTagName("initialRent").Item[0].InnerText;
            //                }

            //                tPriorDt = "";
            //                if (detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item[0])
            //                {
            //                    IsNot;
            //                    null;
            //                    tPriorDt = detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item[0].InnerText;
            //                }

            //                tPriorReas = "";
            //                if (detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item[0])
            //                {
            //                    IsNot;
            //                    null;
            //                    tPriorReas = detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item[0].InnerText;
            //                }

            //                tSmokYN = "";
            //                if (detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item[0])
            //                {
            //                    IsNot;
            //                    null;
            //                    tSmokYN = detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item[0].InnerText;
            //                }

            //                tSmokDt = "";
            //                if (detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item[0])
            //                {
            //                    IsNot;
            //                    null;
            //                    tSmokDt = detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item[0].InnerText;
            //                }

            //                foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
            //                {
            //                    DataRow NR = iTenantsTbl.NewRow();
            //                    NR.Item["TenantID"] = detailOccBy.SelectSingleNode("occupantId").InnerText;
            //                    NR.Item["FirstName"] = detailOccBy.SelectSingleNode("name").SelectSingleNode("firstName").InnerText;
            //                    NR.Item["LastName"] = detailOccBy.SelectSingleNode("name").SelectSingleNode("lastName").InnerText;
            //                    NR.Item["DispName"] = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
            //                    NR.Item["PhoneNo"] = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("phoneNumber").InnerText;
            //                    NR.Item["EmailAddr"] = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("emailAddress").InnerText;
            //                    iTenantsTbl.Rows.Add(NR);
            //                }

            //                tUnitInfo = ("CPStatus="
            //                            + (detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText + ("::HServices="
            //                            + (tServices + ("::StartDt="
            //                            + (tStartDt + ("::NumTenants="
            //                            + (TenCnt.ToString + ("::SmokeYN="
            //                            + (tSmokYN + ("::SmokeDt="
            //                            + (tSmokDt + ("::InitRent="
            //                            + (tInitRent + ("::PriorEndDt="
            //                            + (tPriorDt + ("::TermReason="
            //                            + (tPriorReas + ("::OwnerName="
            //                            + (iBillContact + ("::AgentName="
            //                            + (iAgentName + ("::UnitID="
            //                            + (detailUnits.SelectSingleNode("unitId").InnerText + ("::OwnerEmail=" + iBillEmail)))))))))))))))))))))))));
            //            }

            //        }

            //    }

            //    if ((iStatus.ToUpper != "SUCCESS"))
            //    {
            //        iStatus = "FAILURE";
            //    }

            //    if ((tUnitInfo.Length > 0))
            //    {
            //        return tUnitInfo;
            //    }

            //    return iStatus;
            //}
            //catch (WebException ex)
            //{
            //    iErrMsg = ex.ToString;
            //    // MsgBox(ex.ToString())
            //    return "FAILURE";
            //}

        }

        public static bool SaveCart_Soap(string aSoapStr)
        {
            // TODO: uncomment and convert
            return true;

            //WebRequest Request;
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //// Dim SoapStr As String
            //string status = "";
            //string errMsg = "";
            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(aSoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.SavePaymentCartDetails/RTSClientPo" +
            //        "rtalAPI_API_WSD_SavePaymentCartDetails_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Binder_savePaymentCart");
            //    Request.ContentType = "text/xml; charset=utf-8";
            //    Request.ContentLength = SoapByte.Length;
            //    Request.Method = "POST";
            //    DataStream = Request.GetRequestStream();
            //    DataStream.Write(SoapByte, 0, SoapByte.Length);
            //    DataStream.Close();
            //    Response = Request.GetResponse();
            //    DataStream = Response.GetResponseStream();
            //    Reader = new StreamReader(DataStream);
            //    string SD2Request = Reader.ReadToEnd();
            //    DataStream.Close();
            //    Reader.Close();
            //    Response.Close();
            //    // Set session variables from response
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(SD2Request);
            //    foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("response"))
            //    {
            //        iStatus = detail.ChildNodes(0).InnerText;
            //        if ((iStatus.ToUpper != "SUCCESS"))
            //        {
            //            iErrMsg = detail.ChildNodes(1).InnerText;
            //        }

            //    }

            //    if ((iStatus.ToUpper == "SUCCESS"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}
            //catch (WebException ex)
            //{
            //    iErrMsg = ex.ToString;
            //    // MsgBox(ex.ToString())
            //    return false;
            //}

        }

        public static bool SaveUnit_Soap(string aSoapStr)
        {
            // TODO: uncomment and convert
            return true;

            //WebRequest Request;
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //// Dim SoapStr As String
            //string status = "";
            //string errMsg = "";
            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(aSoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdateUnitStatusChange/RTSClientPo" +
            //        "rtalAPI_API_WSD_UpdateUnitStatusChange_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUnitStatusChange_Binder_updateUnitStatusChange");
            //    Request.ContentType = "text/xml; charset=utf-8";
            //    Request.ContentLength = SoapByte.Length;
            //    Request.Method = "POST";
            //    DataStream = Request.GetRequestStream();
            //    DataStream.Write(SoapByte, 0, SoapByte.Length);
            //    DataStream.Close();
            //    Response = Request.GetResponse();
            //    DataStream = Response.GetResponseStream();
            //    Reader = new StreamReader(DataStream);
            //    string SD2Request = Reader.ReadToEnd();
            //    DataStream.Close();
            //    Reader.Close();
            //    Response.Close();
            //    // Set session variables from response
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(SD2Request);
            //    foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("response"))
            //    {
            //        iStatus = detail.ChildNodes(0).InnerText;
            //        if ((iStatus.ToUpper != "SUCCESS"))
            //        {
            //            iErrMsg = detail.ChildNodes(1).InnerText;
            //        }

            //    }

            //    if ((iStatus.ToUpper == "SUCCESS"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}
            //catch (WebException ex)
            //{
            //    iErrMsg = ex.ToString;
            //    // MsgBox(ex.ToString())
            //    return false;
            //}

        }

        public static bool aSaveTenant_Soap(string aSoapStr)
        {
            // TODO: uncomment and convert
            return true;

            //WebRequest Request;
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //// Dim SoapStr As String
            //string status = "";
            //string errMsg = "";
            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(aSoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdateUnitTenancy/RTSClientPortalA" +
            //        "PI_API_WSD_UpdateUnitTenancy_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Binder_updateUnitTenancy");
            //    Request.ContentType = "text/xml; charset=utf-8";
            //    Request.ContentLength = SoapByte.Length;
            //    Request.Method = "POST";
            //    DataStream = Request.GetRequestStream();
            //    DataStream.Write(SoapByte, 0, SoapByte.Length);
            //    DataStream.Close();
            //    Response = Request.GetResponse();
            //    DataStream = Response.GetResponseStream();
            //    Reader = new StreamReader(DataStream);
            //    string SD2Request = Reader.ReadToEnd();
            //    DataStream.Close();
            //    Reader.Close();
            //    Response.Close();
            //    // Set session variables from response
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(SD2Request);
            //    foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("response"))
            //    {
            //        iStatus = detail.ChildNodes(0).InnerText;
            //        if ((iStatus.ToUpper != "SUCCESS"))
            //        {
            //            iErrMsg = detail.ChildNodes(1).InnerText;
            //        }

            //    }

            //    if ((iStatus.ToUpper == "SUCCESS"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}
            //catch (WebException ex)
            //{
            //    iErrMsg = ex.ToString;
            //    // MsgBox(ex.ToString())
            //    return false;
            //}

        }

        /// <summary>
        /// DONE
        /// </summary>
        public static bool ValidateReset(UserProfile userProfile)
        {
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

            var soapMessage = NewSoapMessage();
            soapMessage.Append("<soapenv:Header/>");
            soapMessage.Append("<soapenv:Body>");
            soapMessage.Append("<api:validateResetUserPassword>");
            soapMessage.Append("<resetUserPwdReq>");
            soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", userProfile.UserCode.EscapeXMLChars());
            soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", userProfile.BillingCode.Length == 0 ? "?" : userProfile.BillingCode.EscapeXMLChars());
            soapMessage.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", userProfile.Question1.EscapeXMLChars());
            soapMessage.AppendFormat("<securityQuestion2>{0}</securityQuestion1>", userProfile.Question2.EscapeXMLChars());
            soapMessage.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", userProfile.Answer1.EscapeXMLChars());
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

                return iStatus.ToUpper() == "SUCCESS";
            }
            catch (WebException ex)
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
