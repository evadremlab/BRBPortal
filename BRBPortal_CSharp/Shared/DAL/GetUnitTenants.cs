using System;
using System.Collections.Generic;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool GetUnitTenants(ref BRBUser user)
        {
            var gotTenants = false;

            var soapRequest = new SoapRequest
            {
                Source = "GetPropertyTenants",
                StaticDataFile = "GetPropertyAndUnitDetails_Response.xml",
                Url = "GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port",
                Action = "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails"
            };

            try
            {
                user.CurrentUnit.Tenants = new List<BRBTenant>();

                soapRequest.Body.Append("<api:getPropertyAndUnitDetails>");
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", SafeString(user.CurrentProperty.PropertyID));
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getPropertyAndUnitDetails>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        var unitID = detailUnits.SelectSingleNode("unitId").InnerText;

                        if (user.CurrentUnit.UnitID == detailUnits.SelectSingleNode("unitId").InnerText)
                        {
                            //
                            // OCCUPANCY
                            //

                            if (detailUnits.SelectNodes("occupancy").Count > 0) // some Rented Units have no Occupants -- bad data?
                            {
                                if (detailUnits.GetElementsByTagName("noOfOccupants").Item(0) != null)
                                {
                                    user.CurrentUnit.TenantCount = int.Parse(detailUnits.GetElementsByTagName("noOfOccupants").Item(0).InnerText);
                                }

                                if (detailUnits.GetElementsByTagName("initialRent").Item(0) != null)
                                {
                                    user.CurrentUnit.InitialRent = detailUnits.GetElementsByTagName("initialRent").Item(0).InnerText;
                                }

                                if (detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0) != null)
                                {
                                    user.CurrentUnit.DatePriorTenancyEnded = DateTime.Parse(detailUnits.GetElementsByTagName("datePriorTenancyEnded").Item(0).InnerText);
                                }

                                if (detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0) != null)
                                {
                                    user.CurrentUnit.ReasonPriorTenancyEnded = detailUnits.GetElementsByTagName("reasonPriorTenancyEnded").Item(0).InnerText;
                                }

                                if (detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0) != null)
                                {
                                    user.CurrentUnit.SmokingProhibitionInLeaseStatus = detailUnits.GetElementsByTagName("smokingProhibitionInLeaseStatus").Item(0).InnerText;
                                }

                                if (detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0) != null)
                                {
                                    DateTime dtSmokingProhibitionEffectiveDate = DateTime.MinValue;

                                    if (DateTime.TryParse(detailUnits.GetElementsByTagName("smokingProhibitionEffectiveDate").Item(0).InnerText, out dtSmokingProhibitionEffectiveDate))
                                    {
                                        user.CurrentUnit.SmokingProhibitionEffectiveDate = dtSmokingProhibitionEffectiveDate;
                                    }
                                }

                                //
                                // TENANTS
                                //

                                foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                                {
                                    var tenant = new BRBTenant();

                                    tenant.TenantID = detailOccBy.SelectSingleNode("occupantId").InnerText;
                                    tenant.FirstName = detailOccBy.SelectSingleNode("name").SelectSingleNode("firstName").InnerText;
                                    tenant.LastName = detailOccBy.SelectSingleNode("name").SelectSingleNode("lastName").InnerText;
                                    tenant.PhoneNumber = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("phoneNumber").InnerText;
                                    tenant.Email = detailOccBy.SelectSingleNode("contactInfo").SelectSingleNode("emailAddress").InnerText;

                                    user.CurrentUnit.Tenants.Add(tenant);
                                }
                            }
                        }
                    }
                }

                gotTenants = true;
            }
            catch (Exception ex)
            {
                SetErrorMessage("GetUnitTenants", ex);
            }

            return gotTenants;
        }
    }
}