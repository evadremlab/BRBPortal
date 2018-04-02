using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp
{
    public partial class ConfirmPayment : System.Web.UI.Page
    {
        protected string BillingCode;
        protected string CartID;
        protected decimal PaymentAmount;

        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Master.User;

            if (!string.IsNullOrEmpty(user.Cart.ID))
            {
                this.CartID = user.Cart.ID;
                this.BillingCode = user.BillingCode;
                this.PaymentAmount = user.Cart.PaymentAmount;
#if DEBUG
                if (this.PaymentAmount == 0)
                {
                    this.PaymentAmount = 10.0M;
                }
#endif
            }
            else
            {
                Master.ShowErrorModal("Cart has no ID", "Error");
            }
        }
    }
}