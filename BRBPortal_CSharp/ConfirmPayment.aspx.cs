using System;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp
{
    public partial class ConfirmPayment : System.Web.UI.Page
    {
        protected string BillingCode;
        protected string CartID;
        protected decimal PaymentAmount;
        public Decimal totalBalance;

        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Master.User;
            var provider = Master.DataProvider;

            this.BillingCode = user.BillingCode;
            this.PaymentAmount = user.Cart.PaymentAmount;
#if DEBUG
            if (this.PaymentAmount == 0)
            {
                this.PaymentAmount = 10.0M;
            }
#endif
            if (!IsPostBack)
            {
                gvCart.DataSource = provider.ConvertToDataTable<BRBCartItem>(user.Cart.Items);
                gvCart.DataBind();
            }
        }

        protected void gvCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var user = Master.User;
            var provider = Master.DataProvider;

            gvCart.PageIndex = e.NewPageIndex;
            gvCart.DataSource = provider.ConvertToDataTable<BRBCartItem>(user.Cart.Items);
            gvCart.DataBind();
        }

        protected void gvCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal value;
                var colIndex = e.Row.GetColumnIndexByName("Balance");
                string txtBalance = e.Row.Cells[colIndex].Text.Replace("$", "");

                if (Decimal.TryParse(txtBalance, out value))
                {
                    totalBalance += value;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = totalBalance.ToString("c");
            }
        }

        protected void CancelCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cart", false);
        }

        protected void PayCart_Click(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;
                var provider = Master.DataProvider;

                if (provider.SaveCart(user))
                {
#if DEBUG
                    user.Cart.ID = "1234";
#endif
                    if (!string.IsNullOrEmpty(user.Cart.ID))
                    {
                        this.CartID = user.Cart.ID;

                        Master.UpdateSession(user);
                        Master.SubmitPaymentForm(user.Cart.ID);
                    }
                    else
                    {
                        Master.ShowErrorModal("Error saving cart. No Cart ID returned.", "Save Cart");
                    }
                }
                else
                {
                    Master.ShowErrorModal("Error saving cart.", "Save Cart");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("PayCart", ex);
            }
        }
    }
}