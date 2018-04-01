using System;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool Register(ref BRBUser user)
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
                    Status = detail.ChildNodes[0].InnerText;

                    if (Status.ToUpper() != "SUCCESS")
                    {
                        ErrorMessage = detail.ChildNodes[1].InnerText;
                    }
                }

                wasRegistered = Status.ToUpper().Equals("SUCCESS");
            }
            catch (Exception ex)
            {
                SetErrorMessage("Register", ex);
            }

            return wasRegistered;
        }
    }
}