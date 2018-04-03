using System;
using System.Xml;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool GetUserProfile(ref BRBUser user)
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

                if (string.IsNullOrEmpty(user.BillingCode))
                {
                    soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                }
                else
                {
                    soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                }

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
                            user.UnitNumber = detailAddr.SelectSingleNode("unitNumber").InnerText;
                            user.FullAddress = detailAddr.SelectSingleNode("fullAddress").InnerText;
                            user.City = detailAddr.SelectSingleNode("city").InnerText;
                            user.StateCode = detailAddr.SelectSingleNode("state").InnerText;
                            user.ZipCode = detailAddr.SelectSingleNode("zip").InnerText;
                            user.Country = detailAddr.SelectSingleNode("country").InnerText;

                            user.Question1 = detail.SelectSingleNode("securityQuestion1").InnerText;
                            user.Question2 = detail.SelectSingleNode("securityQuestion2").InnerText;

                            if ((detailName.SelectSingleNode("agencyName").InnerText.Length > 0))
                            {
                                user.AgencyName = detailName.SelectSingleNode("agencyName").InnerText.UnescapeXMLChars();
                            }

                            gotProfile = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("GetUserProfile", ex);
            }

            return gotProfile;
        }
    }
}