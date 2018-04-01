using System;
using System.Xml;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool UpdateUserProfile(ref BRBUser user)
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
                        ErrorMessage = detail.ChildNodes[2].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Status = "FAILURE";
                SetErrorMessage("UpdateUserProfile", ex);
            }

            return wasUpdated;
        }
    }
}