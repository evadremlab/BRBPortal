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
                soapMessage.AppendFormat("<propertyId>{0}</propertyId>", user.CurrentProperty.PropertyID);
                soapMessage.AppendFormat("<unitId>{0}</unitId>", user.CurrentUnit.UnitID);
                soapMessage.AppendFormat("<unitStatus>{0}</unitStatus>", ""); // TODO
                soapMessage.AppendFormat("<initialRent>{0}</initialRent>", ""); // TODO
                soapMessage.AppendFormat("<tenancyStartDate>{0}</tenancyStartDate>", ""); // TODO
                soapMessage.AppendFormat("<priorTenancyEndDate>{0}</priorTenancyEndDate>", ""); // TODO

                soapMessage.Append("<!--Zero or more repetitions:-->");
                // for each housing services
                soapMessage.Append("<housingServices>");
                soapMessage.AppendFormat("<serviceName>{0}</serviceName>", ""); // TODO
                soapMessage.Append("</housingServices>");

                soapMessage.AppendFormat("<!--Optional:--><otherHousingService>{0}</otherHousingService>", ""); // TODO
                soapMessage.AppendFormat("<noOfTenants>{0}</noOfTenants>", ""); // TODO
                soapMessage.AppendFormat("<smokingProhibitionInLeaseStatus>{0}</smokingProhibitionInLeaseStatus>", ""); // TODO
                soapMessage.AppendFormat("<smokingProhibitionEffectiveDate>{0}</smokingProhibitionEffectiveDate>", ""); // TODO
                soapMessage.AppendFormat("<reasonForTermination>{0}</reasonForTermination>", ""); // TODO
                soapMessage.AppendFormat("<otherReasonForTermination>{0}</otherReasonForTermination>", ""); // TODO
                soapMessage.AppendFormat("<!--Optional:--><explainInvoluntaryTermination>{0}</explainInvoluntaryTermination>", ""); // TODO

                soapMessage.Append("<!--Zero or more repetitions:-->");
                // for each tenant
                soapMessage.Append("<tenants>");
                soapMessage.Append("<code></code>");
                soapMessage.Append("<name>");
                soapMessage.AppendFormat("<first>{0}</first>", ""); // TODO
                soapMessage.Append("<!--Optional:--><middle></middle>");
                soapMessage.AppendFormat("<last>{0}</last>", ""); // TODO
                soapMessage.AppendFormat("<suffix>{0}</suffix>", ""); // TODO
                soapMessage.Append("<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>");
                soapMessage.Append("<!--Optional:--><agencyName></agencyName>");
                soapMessage.Append("</name>");
                soapMessage.AppendFormat("<phoneNumber>{0}</phoneNumber>", ""); // TODO
                soapMessage.AppendFormat("<emailAddress>{0}</emailAddress>", ""); // TODO
                soapMessage.Append("</tenants>");

                soapMessage.Append("</unitTenancyUpdateReq>");
                soapMessage.Append("</api:updateUnitTenancy>");
                soapMessage.Append("</soapenv:Body>");
                soapMessage.Append("</soapenv:Envelope>");

                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    Status = detail.ChildNodes[0].InnerText;

                    if (Status.ToUpper() != "SUCCESS")
                    {
                        Status = detail.ChildNodes[1].InnerText;
                    }
                }

                wasSaved = Status.ToUpper().Equals("SUCCESS");
            }
            catch (Exception ex)
            {
                SetErrorMessage("UpdateUnitTenancy", ex);
            }

            return wasSaved;
        }
    }
}