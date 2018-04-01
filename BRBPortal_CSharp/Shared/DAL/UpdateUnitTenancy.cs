using System;
using System.Xml;
using BRBPortal_CSharp.Models;

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
                var soapMessage = NewSoapMessage();
                soapMessage.Append("<soapenv:Header/>");
                soapMessage.Append("<soapenv:Body>");
                soapMessage.Append("<api:updateUnitTenancy>");
                soapMessage.Append("<unitTenancyUpdateReq>");
                soapMessage.AppendFormat("<userId>{0}</userId>", user.UserCode);
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", property.PropertyID);
                soapMessage.AppendFormat("<unitId>{0}</unitId>", unit.UnitID);
                soapMessage.AppendFormat("<unitStatus>{0}</unitStatus>", unit.ClientPortalUnitStatusCode);
                soapMessage.AppendFormat("<initialRent>{0}</initialRent>", unit.InitialRent);
                soapMessage.AppendFormat("<tenancyStartDate>{0}</tenancyStartDate>", unit.StartDt.Value.ToString("MM/dd/yyyy"));
                soapMessage.AppendFormat("<priorTenancyEndDate>{0}</priorTenancyEndDate>", unit.DatePriorTenancyEnded.Value.ToString("MM/dd/yyyy"));

                soapMessage.Append("<!--Zero or more repetitions:-->");
                foreach (var service in unit.HServices.Split(','))
                {
                    soapMessage.Append("<housingServices>");
                    soapMessage.AppendFormat("<serviceName>{0}</serviceName>", service);
                    soapMessage.Append("</housingServices>");
                }

                soapMessage.AppendFormat("<!--Optional:--><otherHousingService>{0}</otherHousingService>", unit.OtherHServices);
                soapMessage.AppendFormat("<noOfTenants>{0}</noOfTenants>", unit.TenantCount);
                soapMessage.AppendFormat("<smokingProhibitionInLeaseStatus>{0}</smokingProhibitionInLeaseStatus>", unit.SmokingProhibitionInLeaseStatus);

                if (unit.SmokingProhibitionEffectiveDate.HasValue)
                {
                    soapMessage.AppendFormat("<smokingProhibitionEffectiveDate>{0}</smokingProhibitionEffectiveDate>", unit.SmokingProhibitionEffectiveDate.Value.ToString("MM/dd/yyyy"));
                }
                else
                {
                    soapMessage.Append("<smokingProhibitionEffectiveDate></smokingProhibitionEffectiveDate>");
                }

                soapMessage.AppendFormat("<reasonForTermination>{0}</reasonForTermination>", unit.TerminationReason);
                soapMessage.AppendFormat("<otherReasonForTermination>{0}</otherReasonForTermination>", unit.OtherTerminationReason);
                soapMessage.AppendFormat("<!--Optional:--><explainInvoluntaryTermination>{0}</explainInvoluntaryTermination>", unit.OtherTerminationReason);

                soapMessage.Append("<!--Zero or more repetitions:-->");
                foreach (var tenant in unit.Tenants)
                {
                    soapMessage.Append("<tenants>");
                    soapMessage.Append("<code></code>");
                    soapMessage.Append("<name>");
                    soapMessage.AppendFormat("<first>{0}</first>", tenant.FirstName);
                    soapMessage.Append("<!--Optional:--><middle></middle>");
                    soapMessage.AppendFormat("<last>{0}</last>", tenant.LastName);
                    soapMessage.Append("<suffix></suffix>");
                    soapMessage.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                    soapMessage.Append("<!--Optional:--><agencyName></agencyName>");
                    soapMessage.Append("</name>");
                    soapMessage.AppendFormat("<phoneNumber>{0}</phoneNumber>", tenant.PhoneNumber);
                    soapMessage.AppendFormat("<emailAddress>{0}</emailAddress>", tenant.Email);
                    soapMessage.Append("</tenants>");
                }

                soapMessage.AppendFormat("<declarationInitial>{0}</declarationInitial>", unit.DeclarationInitials);

                soapMessage.Append("</unitTenancyUpdateReq>");
                soapMessage.Append("</api:updateUnitTenancy>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasSaved = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasSaved)
                    {
                        Status = detail.ChildNodes[1].InnerText;
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