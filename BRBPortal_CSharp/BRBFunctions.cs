using System.IO;
using System.Net;
using Microsoft.AspNet.Identity.Owin;
using System.Xml;
using System.Text.RegularExpressions;
//using QSILib;
using System.Data;
using System;
using System.Text;

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

        public static SignInStatus UserAuth(string aID, string aBillCd, string aPwd)
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
                return UserAuth_Soap(aID, aBillCd, aPwd);
            }
        }

        /// <summary>
        /// Converted from VB.NET
        /// </summary>
        private static SignInStatus UserAuth_Soap(string aID, string aBillCd, string aPwd)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var signInStatus = SignInStatus.Success;

            var soapMessage = NewSoapMessage();
            soapMessage.Append("<soapenv:Header/>");
            soapMessage.Append("<soapenv:Body>");
            soapMessage.Append("<api:authenticateUserLogin>");
            soapMessage.Append("<authenticateUserReq>");
            soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", aID.Length == 0 ? "?" : EscapeXMLChars(aID));
            soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", aBillCd.Length == 0 ? "?" : EscapeXMLChars(aBillCd));
            soapMessage.AppendFormat("<pwd>{0}</pwd>", EscapeXMLChars(aPwd));
            soapMessage.Append("</authenticateUserReq>");
            soapMessage.Append("</api:authenticateUserLogin>");
            soapMessage.Append("</soapenv:Body>");
            soapMessage.Append("</soapenv:Envelope>");

            try
            {
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
                var SD2Request = reader.ReadToEnd();
                var xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(SD2Request);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("authenticateUserRes"))
                {
                    iStatus = detail.ChildNodes[0].InnerText;

                    // Set session variables from response

                    if (iStatus.ToUpper() == "SUCCESS")
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

                if (iStatus.ToUpper() != "SUCCESS")
                {
                    signInStatus = SignInStatus.Failure;
                }
            }
            catch (Exception ex)
            {
                iErrMsg = ex.ToString();
                signInStatus = SignInStatus.Failure;
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

        public static bool ConfirmProfile_Soap(string aID, string aBillCd, string aInits)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var isConfirmed = true;

            var soapMessage = NewSoapMessage();
            soapMessage.Append("<soapenv:Header/>");
            soapMessage.Append("<soapenv:Body>");
            soapMessage.Append("<api:confirmProfileInformation>");
            soapMessage.Append("<profileConfirmationReq>");
            soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", aID.Length == 0 ? "?" : EscapeXMLChars(aID));
            soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", aBillCd.Length == 0 ? "?" : EscapeXMLChars(aBillCd));
            soapMessage.AppendFormat("<declarationInitial>{0}</declarationInitial>", EscapeXMLChars(aInits));
            soapMessage.Append("</profileConfirmationReq>");
            soapMessage.Append("</api:confirmProfileInformation>");
            soapMessage.Append("</soapenv:Body>");
            soapMessage.Append("</soapenv:Envelope>");

            try
            {
                var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());
                //
                // CONTINUE HERE
                //
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
                var SD2Request = reader.ReadToEnd();
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(SD2Request);

                foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("response"))
                {
                    // Set session variables from response

                    iStatus = detail.SelectSingleNode("status").InnerText;

                    if (iStatus.ToUpper() != "SUCCESS")
                    {
                        iErrMsg = detail.SelectSingleNode("errorMsg").InnerText;
                    }
                }

                if (iStatus.ToUpper() != "SUCCESS")
                {
                    isConfirmed = false;
                }
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

            return isConfirmed;
        }
        //
        // CONTINUE HERE
        //
        public static bool UpdatePassword_Soap(string aID, string aBillCd, string aCurrentPWD, string aNewPWD, string aReTypePWD)
        {
            // TODO: uncomment and convert
            return true;
            //    WebRequest Request;
            //    WebResponse Response;
            //    Stream DataStream;
            //    StreamReader Reader;
            //    byte[] SoapByte;
            //    string SoapStr;
            //    SoapStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:api=\"http://cityofb" +
            //    "erkeley.info/RTS/ClientPortal/API\">";
            //    SoapStr += "<soapenv:Header/>";
            //    SoapStr += "<soapenv:Body>";
            //    SoapStr += "<api:updateUserPassword>";
            //    SoapStr += "<updateUserPwdReq>";
            //    if ((aID.Length > 0))
            //    {
            //        SoapStr = (SoapStr + ("<!--Optional:--><userId>"  + (aID + "</userId>")));
            //    }
            //    else
            //    {
            //        SoapStr += "<!--Optional:--><userId>?</userId>";
            //    }

            //    if ((aBillCd.Length > 0))
            //    {
            //        SoapStr = (SoapStr + ("<!--Optional:--><billingCode>" + (aBillCd + "</billingCode>")));
            //    }
            //    else
            //    {
            //        SoapStr += "<!--Optional:--><billingCode>?</billingCode>";
            //    }

            //    SoapStr = (SoapStr + ("<currentPwd>" + (aCurrentPWD + "</currentPwd>")));
            //    SoapStr = (SoapStr + ("<newPwd>" + (aNewPWD + "</newPwd>")));
            //    SoapStr = (SoapStr + ("<retypeNewPwd>" + (aReTypePWD + "</retypeNewPwd>")));
            //    SoapStr += "</updateUserPwdReq>";
            //    SoapStr += "</api:updateUserPassword>";
            //    SoapStr += "</soapenv:Body>";
            //    SoapStr += "</soapenv:Envelope>";

            //    try
            //    {
            //        SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr);
            //            Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdatePassword/RTSClientPortalAPI_API_WSD_UpdatePassword_Port");
            //        Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdatePassword_Binder_updateUserPassword");
            //        Request.ContentType = "text/xml; charset=utf-8";
            //        Request.ContentLength = SoapByte.Length;
            //        Request.Method = "POST";
            //        DataStream = Request.GetRequestStream();
            //        DataStream.Write(SoapByte, 0, SoapByte.Length);
            //        DataStream.Close();
            //        Response = Request.GetResponse();
            //        DataStream = Response.GetResponseStream();
            //        Reader = new StreamReader(DataStream);
            //        string SD2Request = Reader.ReadToEnd();
            //        DataStream.Close();
            //        Reader.Close();
            //        Response.Close();
            //        // Set session variables from response
            //        XmlDocument doc = new XmlDocument();
            //        doc.LoadXml(SD2Request);
            //        foreach (XmlElement detail in doc.DocumentElement.GetElementsByTagName("response"))
            //        {
            //            iStatus = detail.ChildNodes(0).InnerText;
            //            if (iStatus.ToUpper != "SUCCESS")
            //            {
            //                iErrMsg = detail.ChildNodes(1).InnerText;
            //            }
            //        }

            //        if ((iStatus.ToUpper == "SUCCESS"))
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    catch (WebException ex)
            //    {
            //        iErrMsg = ex.ToString;
            //        // MsgBox(ex.ToString())
            //        return false;
            //    }
        }

        public static string GetProfile(string aID, string aBillCd)
        {
            if (USE_MOCK_SERVICES)
            {
                iStatus = "SUCCESS";

                return "UserCode=UC"
                    + "::BillingCode=BC"
                    + "::FirstName=David"
                    + "::MidName="
                    + "::LastName=Balmer"
                    + "::Suffix="
                    + "::FullName=David Balmer"
                    + "::MailAddr=123 Main Street, Alameda, CA 94501 USA"
                    + "::StNum=123"
                    + "::StName=Main"
                    + "::Unit="
                    + "::FullAddr=123 Main Street, Alameda, CA 94501 USA"
                    + "::City=Alameda"
                    + "::State=CA"
                    + "::Zip=94501"
                    + "::Country=USA"
                    + "::Email=david.b.balmer@transsight.com"
                    + "::Phone=555-555-5555"
                    + "::Question1=What is your name?"
                    + "::Answer1=Dave"
                    + "::Question2=Is your name Dave?"
                    + "::Answer2=Yes"
                    + "::AgentName=secret";
            }
            else
            {
                return GetProfile_Soap(aID, aBillCd);
            }
        }

        /// <summary>
        /// Converted from VB.NET
        /// </summary>
        private static string GetProfile_Soap(string aID, string aBillCd)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;

            string RetStr = "";
            var tUserCode = "";
            var tBillCode = "";
            var tFirst = "";
            var tMid = "";
            var tLast = "";
            var tMailAddr = "";
            var tEmail = "";
            var tPhone = "";
            var tSuffix = "";
            string wstr;
            var tAgent = "";
            var tAnswer1 = "";
            var tAnswer2 = "";
            var tQuestion1 = "";
            var tQuestion2 = "";
            var tSNum = "";
            var tSName = "";
            var tUnit = "";
            var tFullAddr = "";
            var tCity = "";
            var tST = "";
            var tZip = "";
            var tCntry = "";

            // clear session vars
            iStatus = "";

            var soapMessage = NewSoapMessage();
            soapMessage.Append("<soapenv:Header/>");
            soapMessage.Append("<soapenv:Body>");
            soapMessage.Append("<api:getProfileDetails>");
            soapMessage.Append("<request>");
            soapMessage.AppendFormat("<!--Optional:--><userId>{0}</userId>", aID.Length == 0 ? "?" : EscapeXMLChars(aID));
            soapMessage.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", aBillCd.Length == 0 ? "?" : EscapeXMLChars(aBillCd));
            soapMessage.Append("</request>");
            soapMessage.Append("</api:getProfileDetails>");
            soapMessage.Append("</soapenv:Body>");
            soapMessage.Append("</soapenv:Envelope>");

            try
            {
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
                string SD2Request = reader.ReadToEnd();
                var xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(SD2Request);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("profileDetails"))
                {
                    foreach (XmlElement detailName in xmlDoc.DocumentElement.GetElementsByTagName("name"))
                    {
                        foreach (XmlElement detailAddr in xmlDoc.DocumentElement.GetElementsByTagName("mailingAddress"))
                        {
                            iStatus = "SUCCESS";
                            tUserCode = detail.SelectSingleNode("userId").InnerText;
                            tBillCode = detail.SelectSingleNode("billingCode").InnerText;
                            tFirst = detailName.SelectSingleNode("first").InnerText;
                            tLast = detailName.SelectSingleNode("last").InnerText;

                            if ((detailName.SelectSingleNode("middle").InnerText.Length > 0))
                            {
                                tMid = detailName.SelectSingleNode("middle").InnerText;
                            }

                            if ((detailName.SelectSingleNode("suffix").InnerText.Length > 0))
                            {
                                tSuffix = detailName.SelectSingleNode("suffix").InnerText;
                            }

                            if ((detailName.SelectSingleNode("agencyName").InnerText.Length > 0))
                            {
                                tAgent = detailName.SelectSingleNode("agencyName").InnerText;
                            }

                            // Build Mailing address
                            tMailAddr = detailAddr.SelectSingleNode("streetNumber").InnerText;
                            tMailAddr = tMailAddr + " " + detailAddr.SelectSingleNode("streetName").InnerText;
                            if (detailAddr.SelectSingleNode("unitNumber").InnerText.Length > 0)
                            {
                                tMailAddr = tMailAddr + ", " + detailAddr.SelectSingleNode("unitNumber").InnerText;
                            }

                            tMailAddr = tMailAddr + ", " + detailAddr.SelectSingleNode("city").InnerText;
                            tMailAddr = tMailAddr + ", " + detailAddr.SelectSingleNode("state").InnerText;
                            tMailAddr = tMailAddr + " " + detailAddr.SelectSingleNode("zip").InnerText;

                            // Save address components
                            tSNum = detailAddr.SelectSingleNode("streetNumber").InnerText;
                            tSName = detailAddr.SelectSingleNode("streetName").InnerText;
                            tUnit = detailAddr.SelectSingleNode("unitNumber").InnerText;
                            tFullAddr = detailAddr.SelectSingleNode("fullAddress").InnerText;
                            tCity = detailAddr.SelectSingleNode("city").InnerText;
                            tST = detailAddr.SelectSingleNode("state").InnerText;
                            tZip = detailAddr.SelectSingleNode("zip").InnerText;
                            tCntry = detailAddr.SelectSingleNode("country").InnerText;
                            tEmail = detail.SelectSingleNode("emailAddress").InnerText;
                            tPhone = detail.SelectSingleNode("phone").InnerText;
                            tQuestion1 = detail.SelectSingleNode("securityQuestion1").InnerText;
                            tAnswer1 = detail.SelectSingleNode("securityAnswer1").InnerText;
                            tQuestion2 = detail.SelectSingleNode("securityQuestion2").InnerText;
                            tAnswer2 = detail.SelectSingleNode("securityAnswer2").InnerText;
                        }
                    }
                }

                if (iStatus.ToUpper() == "SUCCESS")
                {
                    wstr = tFirst;
                    if (tMid.Length > 0)
                    {
                        wstr = (wstr + (" " + tMid));
                    }

                    wstr += (" " + tLast + " " + tSuffix);
                    UnescapeXMLChars(tFirst);
                    UnescapeXMLChars(tMid);
                    UnescapeXMLChars(tLast);
                    UnescapeXMLChars(wstr);
                    UnescapeXMLChars(tAnswer1);
                    UnescapeXMLChars(tAnswer2);
                    RetStr = "UserCode="
                        + tUserCode + "::BillingCode="
                        + tBillCode + "::FirstName="
                        + UnescapeXMLChars(tFirst) + "::MidName="
                        + tMid + "::LastName="
                        + UnescapeXMLChars(tLast) + "::Suffix="
                        + tSuffix + "::FullName="
                        + UnescapeXMLChars(wstr) + "::MailAddr="
                        + UnescapeXMLChars(tMailAddr) + "::StNum="
                        + UnescapeXMLChars(tSNum) + "::StName="
                        + UnescapeXMLChars(tSName) + "::Unit="
                        + tUnit + "::FullAddr="
                        + tFullAddr + "::City="
                        + tCity + "::State="
                        + tST + "::Zip="
                        + tZip + "::Country="
                        + tCntry + "::Email="
                        + tEmail + "::Phone="
                        + tPhone + "::Question1="
                        + tQuestion1 + "::Answer1="
                        + tAnswer1 + "::Question2="
                        + tQuestion2 + "::Answer2="
                        + tAnswer2 + "::AgentName=" + UnescapeXMLChars(tAgent);
                }
                else
                {
                    // Should be blank since the API failed
                    iStatus = "FAILURE";
                    RetStr = "";
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

            return RetStr;
        }

        public static bool UpdateProfile_Soap(string aXML)
        {
            // TODO: uncomment and convert
            return true;
            //// The incoming XML should have the following structure:
            ////    UserID          - User ID from screen
            ////    Billing Code    - Billing code
            ////    First Name      - 
            ////    Middle Name     -
            ////    Last Name       -
            ////    Suffix          -   
            ////    Name Display    -
            ////    Street Number   -
            ////    Street Name     -
            ////    Unit Number     -
            ////    Full Address    -
            ////    City            -
            ////    State           - 2-character state
            ////    Zip             -
            ////    Country         -
            ////    Email Address   -
            ////    Phone Number    -
            ////    Question 1      -
            ////    Answer 1        -
            ////    Question 2      -
            ////    Answer 2        -
            ////    Agency Name     -
            //// 
            //// Each field should be separated by a double-colon (::)
            //WebRequest Request;
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //string SoapStr;
            //string RetStr = "";
            //string tUserCode;
            //string tBillCode;
            //string tFullName;
            //string tFirst;
            //string tMid;
            //string tLast;
            //string tMailAddr;
            //string tEmail;
            //string tPhone;
            //string tSuffix;
            //string tAnswer1;
            //string tAnswer2;
            //string tQuestion1;
            //string tQuestion2;
            //string tSNum;
            //string tSName;
            //string tUnit;
            //string tFullAddr;
            //string tCity;
            //string tST;
            //string tZip;
            //string tCntry;
            //string wstr;
            //string wstr2;
            //string tAgentName;
            //iStatus = "";
            //tUserCode = "";
            //tBillCode = "";
            //tFullName = "";
            //tFirst = "";
            //tMid = "";
            //tLast = "";
            //tMailAddr = "";
            //tSNum = "";
            //tSName = "";
            //tUnit = "";
            //tFullAddr = "";
            //tCity = "";
            //tST = "";
            //tZip = "";
            //tCntry = "";
            //tEmail = "";
            //tPhone = "";
            //tAnswer1 = "";
            //tAnswer2 = "";
            //tQuestion1 = "";
            //tQuestion2 = "";
            //tSuffix = "";
            //tAgentName = "";
            //wstr2 = aXML;
            //wstr = ParseStr(wstr2, "::");

            //// User Code
            //if ((wstr.Length > 0))
            //{
            //    tUserCode = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Billing Code
            //if ((wstr.Length > 0))
            //{
            //    tBillCode = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// First Name
            //if ((wstr.Length > 0))
            //{
            //    tFirst = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Middle Name
            //if ((wstr.Length > 0))
            //{
            //    tMid = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Last Name
            //if ((wstr.Length > 0))
            //{
            //    tLast = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Suffifx
            //if ((wstr.Length > 0))
            //{
            //    tSuffix = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Name Display - blanks for now
            //wstr = ParseStr(wstr2, "::");
            //// Street Number
            //if ((wstr.Length > 0))
            //{
            //    tSNum = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Street Name
            //if ((wstr.Length > 0))
            //{
            //    tSName = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Unit Number
            //if ((wstr.Length > 0))
            //{
            //    tUnit = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Full Address
            //if ((wstr.Length > 0))
            //{
            //    tMailAddr = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// City
            //if ((wstr.Length > 0))
            //{
            //    tCity = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// State abbrev
            //if ((wstr.Length > 0))
            //{
            //    tST = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Zip
            //if ((wstr.Length > 0))
            //{
            //    tZip = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Country
            //if ((wstr.Length > 0))
            //{
            //    tCntry = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Email
            //if ((wstr.Length > 0))
            //{
            //    tEmail = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Phone Number
            //if ((wstr.Length > 0))
            //{
            //    tPhone = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Question 1
            //if ((wstr.Length > 0))
            //{
            //    tQuestion1 = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Answer 1
            //if ((wstr.Length > 0))
            //{
            //    tAnswer1 = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Question 2
            //if ((wstr.Length > 0))
            //{
            //    tQuestion2 = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Answer 2
            //if ((wstr.Length > 0))
            //{
            //    tAnswer2 = wstr;
            //}

            //wstr = ParseStr(wstr2, "::");
            //// Agent Name
            //if ((wstr.Length > 0))
            //{
            //    tAgentName = wstr;
            //}

            //SoapStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:api=\"http://cityofb" +
            //"erkeley.info/RTS/ClientPortal/API\">";
            //SoapStr += "<soapenv:Header/>";
            //SoapStr += "<soapenv:Body>";
            //SoapStr += "<api:updateUserProfile>";
            //SoapStr += "<updateUserProfileReq>";

            //if ((tUserCode.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><userId>" + (tUserCode + "</userId>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><userId></userId>";
            //}

            //if ((tBillCode.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><billingCode>" + (tBillCode + "</billingCode>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><billingCode></billingCode>";
            //}

            //SoapStr += "<name>";
            //SoapStr = (SoapStr + ("<first>" + (EscapeXMLChars(tFirst) + "</first>")));
            //if ((tMid.Length > 0))
            //{
            //    SoapStr = (SoapStr + ("<!--Optional:--><middle>" + (tMid + "</middle>")));
            //}
            //else
            //{
            //    SoapStr += "<!--Optional:--><middle></middle>";
            //}

            //SoapStr = (SoapStr + ("<last>" + (EscapeXMLChars(tLast) + "</last>")));
            //SoapStr = (SoapStr + ("<suffix>" + (tSuffix + "</suffix>")));
            //SoapStr += "<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>";
            //SoapStr = (SoapStr + ("<!--Optional:--><agencyName>" + (EscapeXMLChars(tAgentName) + "</agencyName>")));
            //SoapStr += "</name>";
            //SoapStr += "<mailingAddress>";
            //SoapStr = (SoapStr + ("<!--Optional:--><streetNumber>" + (tSNum + "</streetNumber>")));
            //SoapStr = (SoapStr + ("<!--Optional:--><streetName>" + (EscapeXMLChars(tSName) + "</streetName>")));
            //SoapStr = (SoapStr + ("<!--Optional:--><unitNumber>" + (tUnit + "</unitNumber>")));
            //SoapStr = (SoapStr + ("<fullAddress>" + (EscapeXMLChars(tFullAddr) + "</fullAddress>")));
            //SoapStr = (SoapStr + ("<!--Optional:--><city>" + (tCity + "</city>")));
            //SoapStr = (SoapStr + ("<!--Optional:--><state>" + (tST + "</state>")));
            //SoapStr = (SoapStr + ("<!--Optional:--><zip>" + (tZip + "</zip>")));
            //SoapStr = (SoapStr + ("<!--Optional:--><country>" + (tCntry + "</country>")));
            //SoapStr += "</mailingAddress>";
            //SoapStr = (SoapStr + ("<emailAddress>" + (tEmail + "</emailAddress>")));
            //SoapStr = (SoapStr + ("<phone>" + (tPhone + "</phone>")));
            //SoapStr = (SoapStr + ("<securityQuestion1>" + (tQuestion1 + "</securityQuestion1>")));
            //SoapStr = (SoapStr + ("<securityAnswer1>" + (EscapeXMLChars(tAnswer1) + "</securityAnswer1>")));
            //SoapStr = (SoapStr + ("<securityQuestion2>" + (tQuestion2 + "</securityQuestion2>")));
            //SoapStr = (SoapStr + ("<securityAnswer2>" + (EscapeXMLChars(tAnswer2) + "</securityAnswer2>")));
            //SoapStr += "</updateUserProfileReq>";
            //SoapStr += "<isActive>Y</isActive>";
            //SoapStr += "</api:updateUserProfile>";
            //SoapStr += "</soapenv:Body>";
            //SoapStr += "</soapenv:Envelope>";

            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.UpdateUserProfile/RTSClientPortalA" +
            //        "PI_API_WSD_UpdateUserProfile_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_UpdateUserProfile_Binder_updateUserProfile");
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
            //        if ((iStatus.ToUpper == "FAILURE"))
            //        {
            //            iErrMsg = detail.ChildNodes(2).InnerText;
            //        }

            //    }

            //    // For Each detail As XmlElement In doc.DocumentElement.GetElementsByTagName("profileDetails")
            //    //     For Each detailName As XmlElement In doc.DocumentElement.GetElementsByTagName("name")
            //    //         For Each detailAddr As XmlElement In doc.DocumentElement.GetElementsByTagName("mailingAddress")
            //    //             iStatus = "SUCCESS"
            //    //         Next detailAddr
            //    //     Next detailName
            //    // Next detail
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

        public static bool Register_Soap(string aSoapStr)
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
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ValidateRegistrationRequest/RTSCli" +
            //        "entPortalAPI_API_WSD_ValidateRegistrationRequest_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI.API.WSD.ValidateRegistrationRequest");
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
            //        if ((iStatus != "SUCCESS"))
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
            //    // MsgBox(ex.ToString())
            //    iErrMsg = ex.ToString;
            //    return false;
            //}
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

        public static bool SaveTenant_Soap(string aSoapStr)
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

        public static bool ValidateReset_Soap(string aSoapStr)
        {
            // TODO: uncomment and convert
            return true;

            //WebRequest Request;
            //WebResponse Response;
            //Stream DataStream;
            //StreamReader Reader;
            //byte[] SoapByte;
            //string SoapStr;
            //SoapStr = aSoapStr;
            //try
            //{
            //    SoapByte = System.Text.Encoding.UTF8.GetBytes(SoapStr);
            //    Request = WebRequest.Create("http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.ValidateResetPasswordRequest/RTSCl" +
            //        "ientPortalAPI_API_WSD_ValidateResetPasswordRequest_Port");
            //    Request.Headers.Add("SOAPAction", "RTSClientPortalAPI_API_WSD_ValidateResetPasswordRequest_Binder_validateResetUserPassword");
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
            //    return false;
            //}

        }

        public static bool CheckPswdRules(string aPwd, string aUserID)
        {
            // TODO: uncomment and convert
            return true;

            //bool RetValue = false;
            //bool tResult = true;
            //bool testB;
            //Regex RegexObj = new Regex("[a-zA-Z0-9!@#$%^&_*]");
            //// Test Length
            //if (((aPwd.Length < 7)
            //            || (aPwd.Length > 20)))
            //{
            //    tResult = false;
            //}

            //testB = false;
            //if ((RegexObj.IsMatch(aPwd) == true))
            //{
            //    testB = true;
            //}

            //if ((testB == false))
            //{
            //    tResult = false;
            //}

            //testB = false;
            //if ((aPwd.IndexOf(aUserID) > -1))
            //{
            //    testB = true;
            //}

            //if ((testB == true))
            //{
            //    tResult = false;
            //}

            //RetValue = tResult;
            //return RetValue;
        }

        /// <summary>
        /// Replace Ampersand, Less than, and Greater than characters with XML versions.
        /// </summary>
        public static string EscapeXMLChars(string str = "")
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");

            return str;
        }

        public static string UnescapeXMLChars(string str = "")
        {
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");

            return str;
        }

        /// <summary>
        /// Returns value up to aDivider, and left truncates aStr through aDivider.
        /// aStartIndex starts searching at that position.
        /// </summary>
        public static string ParseStr(ref string aStr, string aDivider, int aStartIndex = 0)
        {
            int divlen, pos;
            string RetStr;

            if (aStartIndex > aStr.Length)
            {
                RetStr = aStr;
                aStr = "";
                return RetStr;
            }

            divlen = aDivider.Length;
            pos = aStr.IndexOf(aDivider, aStartIndex);

            if (pos == 0)
            {
                aStr = Mid(aStr, divlen + 1);
                return "";
            }

            if (pos > 0)
            {
                RetStr = Left(aStr, pos);
                aStr = Mid(aStr, pos + divlen + 1);
            }
            else
            {
                RetStr = aStr;
                aStr = "";
            }

            return RetStr;
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
