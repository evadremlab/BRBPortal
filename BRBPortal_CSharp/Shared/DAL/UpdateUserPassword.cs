using System;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool UpdateUserPassword(BRBUser user, string currentPassword, string newPassword, string reTypePwd)
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
                        ErrorMessage = detail.ChildNodes[1].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("UpdateUserPassword", ex);
            }

            return wasUpdated;
        }
    }
}