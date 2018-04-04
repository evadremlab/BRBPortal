using System;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool UpdateUnit(ref BRBUser user)
        {
            var wasSaved = false;

            var soapRequest = new SoapRequest
            {
                Source = "SaveUnit",
                Url = "UpdateUnitStatusChange/RTSClientPortalAPI_API_WSD_UpdateUnitStatusChange_Port",
                Action = "RTSClientPortalAPI_API_WSD_UpdateUnitStatusChange_Binder_updateUnitStatusChange"
            };

            try
            {
                var unit = user.CurrentUnit;

                soapRequest.Body.Append("<api:updateUnitStatusChange>");
                soapRequest.Body.Append("<unitStatusChangeReq>");
                soapRequest.Body.AppendFormat("<userId>{0}</userId>", user.UserCode);
                soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", user.CurrentProperty.PropertyID);
                soapRequest.Body.AppendFormat("<unitId>{0}</unitId>", user.CurrentUnit.UnitID);
                soapRequest.Body.AppendFormat("<clientPortalUnitStatusCode>{0}</clientPortalUnitStatusCode>", unit.ClientPortalUnitStatusCode);
                soapRequest.Body.AppendFormat("<unitStatus>{0}</unitStatus>", unit.ClientPortalUnitStatusCode);

                soapRequest.Body.AppendFormat("<!--Optional:--><exemptionReason>{0}</exemptionReason>", unit.ExemptionReason);

                soapRequest.Body.AppendFormat("<declarationInitial>{0}</declarationInitial>", unit.DeclarationInitials);

                soapRequest.Body.Append("<questions>");

                if (unit.StartDate.HasValue)
                {
                    soapRequest.Body.AppendFormat("<!--Optional:--><dateStarted>{0}</dateStarted>", unit.StartDate.Value.AsShortDateFormat());
                }
                else
                {
                    soapRequest.Body.Append("<!--Optional:--><dateStarted></dateStarted>");
                }

                if (unit.AsOfDate.HasValue)
                {
                    soapRequest.Body.AppendFormat("<!--Optional:--><asOfDate>{0}</asOfDate>", unit.AsOfDate.Value.AsShortDateFormat());
                }
                else
                {
                    soapRequest.Body.AppendFormat("<!--Optional:--><asOfDate>{0}</asOfDate>", DateTime.Now.AsShortDateFormat());
                }

                soapRequest.Body.AppendFormat("<!--Optional:--><occupiedBy>{0}</occupiedBy>", unit.OccupiedBy);
                soapRequest.Body.AppendFormat("<!--Optional:--><contractNo>{0}</contractNo>", unit.ContractNo);
                soapRequest.Body.AppendFormat("<!--Optional:--><commeUseDesc>{0}</commeUseDesc>", unit.CommUseDesc);
                soapRequest.Body.AppendFormat("<!--Optional:--><isCommeUseZoned>{0}</isCommeUseZoned>", unit.CommZoneUse);
                soapRequest.Body.AppendFormat("<!--Optional:--><isExclusivelyForCommeUse>{0}</isExclusivelyForCommeUse>", unit.CommResYN);

                // Next 3 removed  when Owner Occupied Exempt Duplex was removed from the Other dropdown
                soapRequest.Body.Append("<!--Optional:--><_x0035_0PercentAsOf31Dec1979></_x0035_0PercentAsOf31Dec1979>");
                soapRequest.Body.Append("<!--Optional:--><ownerOccupantName></ownerOccupantName>"); // OK: vb.net is same
                soapRequest.Body.Append("<!--Zero or more repetitions:--><namesOfownersOfRecord></namesOfownersOfRecord>");

                soapRequest.Body.AppendFormat("<!--Optional:--><nameOfPropertyManagerResiding>{0}</nameOfPropertyManagerResiding>", unit.PropMgrName);
                soapRequest.Body.AppendFormat("<!--Optional:--><emailOfPhoneOfPropertyManagerResiding>{0}</emailOfPhoneOfPropertyManagerResiding>", unit.PMEmailPhone);
                soapRequest.Body.AppendFormat("<!--Optional:--><IsOwnersPrinciplePlaceOfResidence>{0}</IsOwnersPrinciplePlaceOfResidence>", unit.PrincResYN);
                soapRequest.Body.AppendFormat("<!--Optional:--><doesOwnerResideInOtherUnitOfThisUnitProperty>{0}</doesOwnerResideInOtherUnitOfThisUnitProperty>", unit.MultiUnitYN);
                soapRequest.Body.Append("<!--Zero or more repetitions:--><tenantsAndContactInfo>"); // OK: vb.net is same
                soapRequest.Body.AppendFormat("<name>{0}</name>", unit.TenantNames);
                soapRequest.Body.AppendFormat("<contactInfo>{0}</contactInfo>", unit.TenantContacts);
                soapRequest.Body.Append("</tenantsAndContactInfo>");
                soapRequest.Body.Append("</questions>");
                soapRequest.Body.Append("</unitStatusChangeReq>");
                soapRequest.Body.Append("</api:updateUnitStatusChange>");

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
                SetErrorMessage("UpdateUnit", ex);
            }

            return wasSaved;
        }
    }
}