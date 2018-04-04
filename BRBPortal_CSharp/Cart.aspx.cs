using System;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;
using System.Collections.Generic;

namespace BRBPortal_CSharp
{
    public partial class Cart : System.Web.UI.Page
    {
        public Decimal totalBalance;
        public string backUrl = "~/Home";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;
                var provider = Master.DataProvider;
                var cartItems = new List<BRBCartItem>();

                user.FeeOption = Session["FeeOption"] as string ?? "All Fees and Penalties";

                Master.UpdateSession(user);

                try
                {
                    foreach (var item in user.Cart.Items)
                    {
                        if (user.FeeOption == "Fees Only")
                        {
                            item.Balance = (item.CurrentFee + item.PriorFee);
                        }
                        else
                        {
                            item.Balance = (item.CurrentFee + item.PriorFee + item.CurrentPenalty + item.PriorPenalty + item.Credits);
                        }

                        cartItems.Add(item);
                    }

                    gvCart.DataSource = provider.ConvertToDataTable<BRBCartItem>(cartItems);
                    gvCart.DataBind();
                }
                catch (Exception ex)
                {
                    Master.ShowErrorModal(ex.Message, "Cart View");
                }

                if (user.Cart.Items.Count == 0)
                {
                    btnEdCart.Enabled = false;
                    btnPayCart.Enabled = false;

                    gvCart.Visible = false;
                    EmptyCartHeader.Visible = true;
                    EmptyCartMessage.Text = "is empty";
                    return;
                }
                else
                {
                    gvCart.Visible = true;
                    EmptyCartMessage.Text = "";
                    EmptyCartHeader.Visible = false;
                }

                if (!IsPostBack)
                {
                    btnEdCart.Enabled = true;
                    btnPayCart.Enabled = true;

                    FeeOption.Text = user.FeeOption;
                }
            }
            catch (Exception ex)
            {
                Master.ShowErrorModal(ex.Message, "Cart");
            }
        }

        protected void CancelCart_Click(object sender, EventArgs e)
        {
            var backUrl = Session["BackFromCartUrl"] as string ?? "~/Home";

            Session["FeeOption"] = "All Fees and Penalties";
            Session.Remove("BackFromCartUrl");

            Response.Redirect(backUrl, false);
        }

        protected void PayCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ConfirmPayment");
        }

        protected void EditCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditCart", false);
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
    }
}