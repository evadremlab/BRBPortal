using System;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool UpdateUnitTenancy(ref BRBUser user)
        {
            var wasSaved = false;
            var property = user.CurrentProperty;
            var unit = user.CurrentUnit;

            var soapRequest = new SoapRequest
            {
                Source = "SaveTenant",
                Url = "UpdateUnitTenancy/RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdateUnitTenancy_Binder_updateUnitTenancy"
            };

            try
            {
                soapRequest.Body.Append("<api:updateUnitTenancy>");
                soapRequest.Body.Append("<unitTenancyUpdateReq>");
                soapRequest.Body.AppendFormat("<userId>{0}</userId>", user.UserCode);
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", property.PropertyID);
                soapRequest.Body.AppendFormat("<unitId>{0}</unitId>", unit.UnitID);
                soapRequest.Body.AppendFormat("<unitStatus>{0}</unitStatus>", unit.ClientPortalUnitStatusCode);
                soapRequest.Body.AppendFormat("<initialRent>{0}</initialRent>", unit.InitialRent);
                soapRequest.Body.AppendFormat("<tenancyStartDate>{0}</tenancyStartDate>", unit.TenancyStartDate.Value.ToString("MM/dd/yyyy"));
                soapRequest.Body.AppendFormat("<priorTenancyEndDate>{0}</priorTenancyEndDate>", unit.DatePriorTenancyEnded.Value.ToString("MM/dd/yyyy"));

                soapRequest.Body.Append("<!--Zero or more repetitions:-->");
                foreach (var service in unit.HServices.Split(','))
                {
                    soapRequest.Body.Append("<housingServices>");
                    soapRequest.Body.AppendFormat("<serviceName>{0}</serviceName>", service);
                    soapRequest.Body.Append("</housingServices>");
                }

                soapRequest.Body.AppendFormat("<!--Optional:--><otherHousingService>{0}</otherHousingService>", unit.OtherHServices);
                soapRequest.Body.AppendFormat("<noOfTenants>{0}</noOfTenants>", unit.Tenants.Count);
                soapRequest.Body.AppendFormat("<smokingProhibitionInLeaseStatus>{0}</smokingProhibitionInLeaseStatus>", unit.SmokingProhibitionInLeaseStatus);

                if (unit.SmokingProhibitionEffectiveDate.HasValue)
                {
                    soapRequest.Body.AppendFormat("<smokingProhibitionEffectiveDate>{0}</smokingProhibitionEffectiveDate>", unit.SmokingProhibitionEffectiveDate.Value.ToString("MM/dd/yyyy"));
                }
                else
                {
                    soapRequest.Body.Append("<smokingProhibitionEffectiveDate></smokingProhibitionEffectiveDate>");
                }

                soapRequest.Body.AppendFormat("<reasonForTermination>{0}</reasonForTermination>", unit.TerminationReason);
                soapRequest.Body.AppendFormat("<otherReasonForTermination>{0}</otherReasonForTermination>", unit.OtherTerminationReason);
                soapRequest.Body.AppendFormat("<!--Optional:--><explainInvoluntaryTermination>{0}</explainInvoluntaryTermination>", unit.OtherTerminationReason);

                soapRequest.Body.Append("<!--Zero or more repetitions:-->");
                foreach (var tenant in unit.Tenants)
                {
                    soapRequest.Body.Append("<tenants>");
                    soapRequest.Body.Append("<code></code>");
                    soapRequest.Body.Append("<name>");
                    soapRequest.Body.AppendFormat("<first>{0}</first>", tenant.FirstName);
                    soapRequest.Body.Append("<!--Optional:--><middle></middle>");
                    soapRequest.Body.AppendFormat("<last>{0}</last>", tenant.LastName);
                    soapRequest.Body.Append("<suffix></suffix>");
                    soapRequest.Body.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                    soapRequest.Body.Append("<!--Optional:--><agencyName></agencyName>");
                    soapRequest.Body.Append("</name>");
                    soapRequest.Body.AppendFormat("<phoneNumber>{0}</phoneNumber>", tenant.PhoneNumber);
                    soapRequest.Body.AppendFormat("<emailAddress>{0}</emailAddress>", tenant.Email);
                    soapRequest.Body.Append("</tenants>");
                }

                soapRequest.Body.AppendFormat("<declarationInitial>{0}</declarationInitial>", unit.DeclarationInitials);

                soapRequest.Body.Append("</unitTenancyUpdateReq>");
                soapRequest.Body.Append("</api:updateUnitTenancy>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasSaved = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasSaved)
                    {
                        ErrorMessage = detail.ChildNodes[1].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("UpdateUnitTenancy", ex);
            }

            return wasSaved;
        }
    }
}