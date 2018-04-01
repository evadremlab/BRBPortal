using System;
using System.Collections.Generic;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool GetPropertyUnits(ref BRBUser user, string unitID = "")
        {
            var gotPropertyUnits = false;

            var soapRequest = new SoapRequest
            {
                Source = "GetPropertyUnits",
                StaticDataFile = "GetPropertyAndUnitDetails_Response.xml",
                Url = "GetPropertyAndUnitDetails/RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Port",
                Action = "RTSClientPortalAPI_API_WSD_GetPropertyAndUnitDetails_Binder_getPropertyAndUnitDetails"
            };

            try
            {
                soapRequest.Body.Append("<api:getPropertyAndUnitDetails>");
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", SafeString(user.CurrentProperty.PropertyID));
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getPropertyAndUnitDetails>");

                var xmlDoc = GetXmlResponse(soapRequest);

                user.CurrentProperty.Units = new List<BRBUnit>();

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("propertyAndUnitsRes"))
                {
                    user.CurrentProperty.OwnerContactName = detail.SelectSingleNode("ownerContactName").SelectSingleNode("nameLastFirstDisplay").InnerText;
                    user.CurrentProperty.BillingAddress = detail.SelectSingleNode("billingDetails").SelectSingleNode("billingAddress").SelectSingleNode("mainStreetAddress").InnerText;

                    if (detail.SelectSingleNode("agentDetails") != null)
                    {
                        if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName") != null)
                        {
                            user.AgencyName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agentContactName").SelectSingleNode("nameLastFirstDisplay").InnerText.UnescapeXMLChars();
                        }

                        if (user.AgencyName.Length < 1)
                        {
                            if (detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName") != null)
                            {
                                user.AgencyName = detail.SelectSingleNode("agentDetails").SelectSingleNode("agencyName").InnerText.UnescapeXMLChars();
                            }
                        }
                    }

                    foreach (XmlElement detailUnits in detail.GetElementsByTagName("units"))
                    {
                        DateTime startDate = DateTime.MinValue;
                        Decimal rentCeiling = 0;
                        var exemptionReason = "";
                        var cpUnitStatDisp = "";
                        var hServices = "";
                        var occupiedBy = "";

                        var unit = new BRBUnit();

                        if (detailUnits.SelectSingleNode("tenancyStartDate") != null)
                        {
                            if (!string.IsNullOrEmpty(detailUnits.SelectSingleNode("tenancyStartDate").InnerText))
                            {
                                unit.StartDt = DateTime.Parse(detailUnits.SelectSingleNode("tenancyStartDate").InnerText);
                            }
                        }

                        if ((detailUnits.SelectSingleNode("rentCeiling").InnerText.Length > 0))
                        {
                            Decimal.TryParse(detailUnits.SelectSingleNode("rentCeiling").InnerText, out rentCeiling);
                        }

                        if (detailUnits.SelectSingleNode("unitStatusAsOfDate") != null)
                        {
                            if (!string.IsNullOrEmpty(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText))
                            {
                                DateTime.TryParse(detailUnits.SelectSingleNode("unitStatusAsOfDate").InnerText, out startDate);
                            }
                        }

                        foreach (XmlElement detailService in detailUnits.GetElementsByTagName("housingServices"))
                        {
                            if (hServices.Length > 0)
                            {
                                hServices += (", " + detailService.SelectSingleNode("serviceName").InnerText);
                            }
                            else
                            {
                                hServices = detailService.SelectSingleNode("serviceName").InnerText;
                            }
                        }

                        switch (unit.UnitStatCode.ToUpper())
                        {
                            case "OOCC":
                                cpUnitStatDisp = "Owner-Occupied";
                                break;
                            case "SEC8":
                                cpUnitStatDisp = "Section 8";
                                break;
                            case "RENTED":
                                cpUnitStatDisp = "Rented or Available for Rent";
                                break;
                            case "FREE":
                                cpUnitStatDisp = "Rent-Free";
                                break;
                            case "NAR":
                                cpUnitStatDisp = "Not Available for Rent";
                                break;
                            case "SPLUS":
                                cpUnitStatDisp = "Shelter Plus";
                                break;
                            case "DUPLEX":
                                cpUnitStatDisp = "Owner-occupied Duplex";
                                break;
                            case "COMM":
                                cpUnitStatDisp = "Commercial";
                                break;
                            case "SHARED":
                                cpUnitStatDisp = "Owner Shares Kit/Bath";
                                break;
                            case "MISC":
                                cpUnitStatDisp = "Miscellaneous Exempt";
                                break;
                        }

                        foreach (XmlElement detailOccBy in detailUnits.GetElementsByTagName("occupants"))
                        {
                            if (occupiedBy.Length > 0)
                            {
                                occupiedBy += (", " + detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText);
                            }
                            else
                            {
                                occupiedBy = detailOccBy.SelectSingleNode("name").SelectSingleNode("nameLastFirstDisplay").InnerText;
                            }
                        }

                        switch (unit.UnitStatCode.ToUpper())
                        {
                            case "OOCC":
                                exemptionReason = "Owner-Occupied";
                                break;
                            case "SEC8":
                                exemptionReason = "Section 8";
                                break;
                            case "RENTED":
                                exemptionReason = "Rented or Available for Rent";
                                break;
                            case "FREE":
                                exemptionReason = "Rent-Free";
                                break;
                            case "NAR":
                                exemptionReason = "Not Available for Rent";
                                break;
                            case "SPLUS":
                                exemptionReason = "Shelter Plus";
                                break;
                            case "DUPLEX":
                                exemptionReason = "Owner-occupied Duplex";
                                break;
                            case "COMM":
                                exemptionReason = "Commercial";
                                break;
                            case "SHARED":
                                exemptionReason = "Owner Shares Kit/Bath";
                                break;
                            case "MISC":
                                exemptionReason = "Miscellaneous Exempt";
                                break;
                        }

                        unit.ClientPortalUnitStatusCode = detailUnits.SelectSingleNode("clientPortalUnitStatusCode").InnerText;
                        //unit.CommResYN = "";
                        //unit.CommUseDesc = "";
                        //unit.CommZoneUse = "";
                        //unit.ContractNo = "";
                        unit.CPUnitStatDisp = cpUnitStatDisp;
                        //unit.DatePriorTenancyEnded = "";
                        //unit.DeclarationInitials = "";
                        unit.ExemptionReason = exemptionReason;
                        unit.HServices = hServices;
                        unit.InitialRent = ""; // part of <occupancy>
                        unit.MultiUnitYN = "";
                        //unit.NumberOfTenants = "";
                        unit.OccupiedBy = occupiedBy;
                        unit.OtherUnits = "";
                        unit.PMEmailPhone = "";
                        unit.PrincResYN = "";
                        unit.PropMgrName = "";
                        unit.ReasonPriorTenancyEnded = "";
                        unit.RentCeiling = rentCeiling;
                        unit.SmokeDetector = "";
                        //unit.SmokingProhibitionEffectiveDate = "";
                        unit.SmokingProhibitionInLeaseStatus = "";

                        if (startDate != DateTime.MinValue)
                        {
                            unit.StartDt = startDate;
                        }

                        unit.StreetAddress = detailUnits.SelectSingleNode("unitStreetAddress").InnerText;
                        unit.TenantContacts = "";
                        unit.TenantNames = unit.TenantNames;
                        unit.TenantContacts = unit.TenantContacts;
                        unit.TenantCount = 0;

                        if (detailUnits.SelectNodes("occupancy").Count > 0)
                        {
                            if (detailUnits.GetElementsByTagName("noOfOccupants").Item(0) != null)
                            {
                                unit.TenantCount = int.Parse(detailUnits.GetElementsByTagName("noOfOccupants").Item(0).InnerText);
                            }
                        }

                        unit.TerminationReason = "";
                        //unit.UnitAsOfDt = "";
                        unit.UnitID = detailUnits.SelectSingleNode("unitId").InnerText;
                        unit.UnitNo = detailUnits.SelectSingleNode("unitNumber").InnerText;
                        unit.UnitStatCode = detailUnits.SelectSingleNode("unitStatusCode").InnerText;
                        unit.UnitStatID = detailUnits.SelectSingleNode("unitStatusId").InnerText;

                        if (string.IsNullOrEmpty(unitID) || unitID == unit.UnitID)
                        {
                            user.CurrentProperty.Units.Add(unit);
                        }
                    }
                }

                gotPropertyUnits = true;
            }
            catch (Exception ex)
            {
                SetErrorMessage("GetPropertyUnits", ex);
            }

            return gotPropertyUnits;
        }
    }
}