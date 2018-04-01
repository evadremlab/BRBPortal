using System;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.DAL;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp
{
    public partial class Cart : System.Web.UI.Page
    {
        public Decimal totalBalance;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;
                var provider = new DataProvider();

                gvCart.DataSource = provider.ConvertToDataTable<BRBCartItem>(user.Cart.Items);
                gvCart.DataBind();

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

                    if (string.IsNullOrEmpty(user.FeesAll))
                    {
                        ShowFeesAll.Text = "All Fees and Penalties";
                    }
                    else
                    {
                        if (user.FeesAll == "AllFees")
                        {
                            ShowFeesAll.Text = "All Fees and Penalties";
                        }
                        else
                        {
                            ShowFeesAll.Text = "Fees Only";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Master.ShowErrorModal(ex.Message, "Cart");
            }
        }

        protected void CancelCart_Click(object sender, EventArgs e)
        {
            Session["FeesAll"] = "AllFees";
            Response.Redirect("~/MyProperties/MyProperties", false);
        }

        protected void PayCart_Click(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;
                var provider = new DataProvider();

                if (provider.SaveCart(user))
                {
                    if (user.Cart.ID.HasValue)
                    {
                        Master.UpdateSession(user);
                        Response.Redirect("~/ConfirmPayment", false);
                    }
                    else
                    {
                        Master.ShowErrorModal("Error saving cart(1)", "Save Cart");
                    }
                }
                else
                {
                    Master.ShowErrorModal("Error saving cart(2).", "Save Cart");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("PayCart", ex);
            }
        }

        protected void EditCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditCart", false);
        }

        protected void gvCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var user = Master.User;
            var provider = new DataProvider();

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
    }
}