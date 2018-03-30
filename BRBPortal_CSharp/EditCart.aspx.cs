using BRBPortal_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp
{
    public partial class EditCart : System.Web.UI.Page
    {
        public DataTable iCartTbl;
        public Decimal iBalance;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;

                gvCart.DataSource = BRBFunctions_CSharp.ConvertToDataTable<BRBCartItem>(user.Cart.Items);
                gvCart.DataBind();
            }
            catch (Exception ex)
            {
                Master.ShowErrorModal(ex.Message, "Edit Cart View");
            }
        }

        protected void gvCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var user = Master.User;

            gvCart.PageIndex = e.NewPageIndex;
            gvCart.DataSource = BRBFunctions_CSharp.ConvertToDataTable<BRBCartItem>(user.Cart.Items);
            gvCart.DataBind();
        }

        protected void gvCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal value;
                string txtBalance = e.Row.Cells[7].Text.Replace("$", "");

                if (Decimal.TryParse(txtBalance, out value))
                {
                    iBalance += value;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = iBalance.ToString("c");
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
    }
}