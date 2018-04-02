using System;
using System.Xml;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool UpdateSecurityQuestions(ref BRBUser user)
        {
            var wasUpdated = false;

            var soapRequest = new SoapRequest
            {
                Source = "UpdateSecurityQuestions",
                Url = "UpdateUserProfileSecurityQA/RTSClientPortalAPI_API_WSD_UpdateUserProfileSecurityQA_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdateUserProfileSecurityQA_Binder_updateUserProfileSecurityQA"
            };

            try
            {
                soapRequest.Body.Append("<api:updateUserProfileSecurityQA>");
                soapRequest.Body.Append("<UpdateSecurityQAReq>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.AppendFormat("<securityQuestion1>{0}</securityQuestion1>", user.Question1.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityAnswer1>{0}</securityAnswer1>", user.Answer1.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityQuestion2>{0}</securityQuestion2>", user.Question2.EscapeXMLChars());
                soapRequest.Body.AppendFormat("<securityAnswer2>{0}</securityAnswer2>", user.Answer2.EscapeXMLChars());
                soapRequest.Body.Append("</UpdateSecurityQAReq>");
                soapRequest.Body.Append("</api:updateUserProfileSecurityQA>");
                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasUpdated = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasUpdated)
                    {
                        ErrorMessage = detail.ChildNodes[1].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("ConfirmUserProfile", ex);
            }

            return wasUpdated;
        }
    }
}