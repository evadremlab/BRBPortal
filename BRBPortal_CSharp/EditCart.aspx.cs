using System;
using System.Data;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.Models;
using System.Collections.Generic;

namespace BRBPortal_CSharp
{
    public partial class EditCart : System.Web.UI.Page
    {
        public Decimal totalBalance;

        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Master.User;
            var provider = Master.DataProvider;
            var cartItems = new List<BRBCartItem>();

            if (!IsPostBack)
            {
                FeeOption.SelectedValue = user.FeeOption;
                hdnFeeOption.Value = user.FeeOption;
                UpdateCartView();
            }
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

        protected void gvCart_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName.Equals("RemoveFromCart"))
            {
                try
                {
                    var user = Master.User;
                    var cartItem = user.Cart.Items[rowIndex];

                    user.Cart.Items.RemoveAll(x => x.PropertyId == cartItem.PropertyId);

                    user.Cart.WasUpdated = true;

                    Master.UpdateSession(user);

                    Response.Redirect("~/Cart", false);
                }
                catch (Exception ex)
                {
                    Master.ShowErrorModal("Error removing cart item: " + ex.Message, "Remove cart item");
                }
            }
        }

        protected void UpdateCart_Click(object sender, EventArgs e)
        {
            var user = Master.User;

            user.FeeOption = hdnFeeOption.Value;
            Session["FeeOption"] = user.FeeOption;

            Master.UpdateSession(user);

            Response.Redirect("~/Cart", false);
        }

        protected void FeeOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnFeeOption.Value = FeeOption.SelectedItem.Text;
            UpdateCartView();
        }

        private void UpdateCartView()
        {
            var user = Master.User;
            var provider = Master.DataProvider;
            var cartItems = new List<BRBCartItem>();

            try
            {
                foreach (var item in user.Cart.Items)
                {
                    if (hdnFeeOption.Value == "Fees Only")
                    {
                        item.Balance = item.CurrentFee + item.PriorFee;
                    }
                    else
                    {
                        item.Balance = item.CurrentFee + item.PriorFee + item.CurrentPenalty + item.PriorPenalty + item.Credits;
                    }

                    cartItems.Add(item);
                }

                gvCart.DataSource = provider.ConvertToDataTable<BRBCartItem>(cartItems);
                gvCart.DataBind();

            }
            catch (Exception ex)
            {
                Master.ShowErrorModal(ex.Message, "Update Cart View");
            }
        }
    }
}