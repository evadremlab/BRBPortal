using System;
using System.Xml;
using Microsoft.AspNet.Identity.Owin;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public SignInStatus Authenticate(ref BRBUser user, string password)
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
                        ErrorMessage = detail.SelectSingleNode("errorMsg").InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("Authenticate", ex);
            }

            return signInStatus;
        }
    }
}