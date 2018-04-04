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

            this.PaymentAmount = 0;
            this.BillingCode = user.BillingCode;

            foreach (var item in user.Cart.Items)
            {
                this.PaymentAmount += item.Balance;
            }

#if DEBUG
            if (this.PaymentAmount == 0)
            {
                this.PaymentAmount = 10.0M;
            }
#endif
            litPaymentAmount.Text = this.PaymentAmount.ToString("C");

            if (!IsPostBack)
            {
                FeeOption.Text = user.FeeOption;
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
                string txtBalance = e.Row.Cells[e.Row.GetColumnIndexByName("Balance")].Text.Replace("$", "");

                if (txtBalance.Contains("("))
                {
                    txtBalance = txtBalance.Replace("(", "-").Replace(")", "");
                }

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