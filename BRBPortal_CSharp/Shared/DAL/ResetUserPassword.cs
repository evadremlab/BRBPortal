using System;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool ResetUserPassword(ref BRBUser user)
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
                        ErrorMessage = detail.ChildNodes[1].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("ResetUserPassword", ex);
            }

            return canReset;
        }
    }
}