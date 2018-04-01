using System;
using System.Collections.Generic;
using System.Xml;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool GetProperties(ref BRBUser user)
        {
            var gotUserProperties = false;

            var soapRequest = new SoapRequest
            {
                Source = "GetUserProperties",
                StaticDataFile = "GetUserProperties_Response.xml",
                Url = "GetUserProfilePropertiesList/RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Port",
                Action = "RTSClientPortalAPI_API_WSD_GetUserProfilePropertiesList_Binder_getProfileProperties"
            };

            try
            {
                soapRequest.Body.Append("<api:getProfileProperties>");
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:getProfileProperties>");

                var xmlDoc = GetXmlResponse(soapRequest);
                user.Properties = new List<BRBProperty>();

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    foreach (XmlElement detailProperty in detail.GetElementsByTagName("profileProperty"))
                    {
                        foreach (XmlElement balanceAmounts in detailProperty.GetElementsByTagName("balanceAmounts"))
                        {
                            decimal currentFees = 0;
                            decimal priorFees = 0;
                            decimal currentPenalty = 0;
                            decimal priorPenalty = 0;
                            decimal credit = 0;
                            decimal totalBalance = 0;
                            var myProperty = new BRBProperty();

                            Decimal.TryParse(balanceAmounts.SelectSingleNode("currentFees").InnerText, out currentFees);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("priorFees").InnerText, out priorFees);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("currentPenalty").InnerText, out currentPenalty);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("priorPenalties").InnerText, out priorPenalty);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("credit").InnerText, out credit);
                            Decimal.TryParse(balanceAmounts.SelectSingleNode("totalBalance").InnerText, out totalBalance);

                            gotUserProperties = detail.SelectSingleNode("status").InnerText.ToUpper().Equals("SUCCESS");

                            if (!gotUserProperties)
                            {
                                ErrorMessage = detail.SelectSingleNode("errorMsg").InnerText;
                            }
                            else
                            {
#if DEBUG
                                if (currentFees == 0)
                                {
                                    currentFees = 10.0M;
                                }
                                if (totalBalance == 0)
                                {
                                    totalBalance = 10.0M;
                                }
#endif
                                myProperty.PropertyID = detailProperty.SelectSingleNode("propertyId").InnerText;
                                myProperty.PropertyAddress = detailProperty.SelectSingleNode("address").SelectSingleNode("mainStreetAddress").InnerText;
                                myProperty.CurrentFee = currentFees;
                                myProperty.PriorFee = priorFees;
                                myProperty.CurrentPenalty = currentPenalty;
                                myProperty.PriorPenalty = priorPenalty;
                                myProperty.Credits = credit;
                                myProperty.Balance = totalBalance;

                                user.Properties.Add(myProperty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("GetProperties", ex);
            }

            return gotUserProperties;
        }
    }
}