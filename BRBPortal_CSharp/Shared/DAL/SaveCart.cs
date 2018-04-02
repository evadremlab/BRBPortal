using System;
using System.Xml;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        public bool SaveCart(BRBUser user)
        {
            var wasSaved = false;

            var soapRequest = new SoapRequest
            {
                Source = "SaveCart",
                StaticDataFile = "SaveCart_Response.xml",
                Url = "SavePaymentCartDetails/RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Port",
                Action = "RTSClientPortalAPI_API_WSD_SavePaymentCartDetails_Binder_savePaymentCart"
            };

            try
            {
                soapRequest.Body.Append("<api:savePaymentCart>");
                soapRequest.Body.Append("<savePaymentToCart>");
                soapRequest.Body.AppendFormat("<cartId>{0}</cartId>", user.Cart.ID);
                soapRequest.Body.AppendFormat("<!--Optional:--><paymentConfirmationNo>{0}</paymentConfirmationNo>", user.Cart.PaymentConfirmationNo);
                soapRequest.Body.AppendFormat("<!--Optional:--><paymentMethod>{0}</paymentMethod>", SafeString(user.Cart.PaymentMethod));
                soapRequest.Body.AppendFormat("<paymentReceivedAmt>{0}</paymentReceivedAmt>", user.Cart.PaymentReceivedAmt);
                soapRequest.Body.AppendFormat("<isFeeOnlyPaid>{0}</isFeeOnlyPaid>", user.Cart.isFeeOnlyPaid ? "true" : "false");
                soapRequest.Body.Append("<!--1 or more repetitions:-->");

                foreach (var cartItem in user.Cart.Items)
                {
                    soapRequest.Body.Append("<items>");
                    soapRequest.Body.AppendFormat("<itemId>{0}</itemId>", cartItem.ID);
                    soapRequest.Body.AppendFormat("<propertyId>{0}</propertyId>", SafeString(cartItem.PropertyId));
                    soapRequest.Body.AppendFormat("<propertyMainStreetAddress>{0}</propertyMainStreetAddress>", SafeString(cartItem.PropertyAddress));
                    soapRequest.Body.AppendFormat("<fee>{0}</fee>", cartItem.SumCurrentFees());
                    soapRequest.Body.AppendFormat("<penalties>{0}</penalties>", cartItem.SumPenalties(user.Cart.isFeeOnlyPaid));
                    soapRequest.Body.AppendFormat("<balance>{0}</balance>", cartItem.SumBalance(user.Cart.isFeeOnlyPaid));
                    soapRequest.Body.Append("</items>");
                }

                soapRequest.Body.Append("</savePaymentToCart>");
                soapRequest.Body.Append("<request>");
                soapRequest.Body.AppendFormat("<!--Optional:--><userId>{0}</userId>", SafeString(user.UserCode));
                soapRequest.Body.AppendFormat("<!--Optional:--><billingCode>{0}</billingCode>", SafeString(user.BillingCode));
                soapRequest.Body.Append("</request>");
                soapRequest.Body.Append("</api:savePaymentCart>");
                var xmlDoc = GetXmlResponse(soapRequest);

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("response"))
                {
                    wasSaved = detail.ChildNodes[0].InnerText.ToUpper().Equals("SUCCESS");

                    if (!wasSaved)
                    {
                        ErrorMessage = detail.ChildNodes[1].InnerText;
                    }
                }

                if (wasSaved)
                {
                    foreach (XmlElement el in xmlDoc.DocumentElement.GetElementsByTagName("cartId"))
                    {
                        user.Cart.ID = el.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("SaveCart", ex);
            }

            return wasSaved;
        }
    }
}