using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace BRBPortal_CSharp
{
    public partial class Cart : System.Web.UI.Page
    {
        public Decimal totalBalance = 0.0M;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;

                gvCart.DataSource = BRBFunctions_CSharp.ConvertToDataTable<BRBCartItem>(user.Cart.Items);
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
                    ShowFeesAll.Text = "ShowFeesAll is under construction";

                    //if (user.FeesAll == null)
                    //{
                    //    ShowFeesAll.Text = "All Fees and Penalties";
                    //}
                    //else
                    //{
                    //    if (Session["FeesAll"].ToString() == "AllFees" || Session["FeesAll"].ToString() == "")
                    //    {
                    //        ShowFeesAll.Text = "All Fees and Penalties";
                    //    }
                    //    else
                    //    {
                    //        ShowFeesAll.Text = "Fees Only";
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Master.ShowDialogOK(ex.Message, "Cart");
            }
        }

        protected void CancelCart_Click(object sender, EventArgs e)
        {
            Session["FeesAll"] = "AllFees";
            Response.Redirect("~/MyProperties/MyProperties");
        }

        protected void PayCart_Click(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;

                if (BRBFunctions_CSharp.SaveCart(user))
                {
                    Logger.Log("SaveCart", "Saved!");
                }
                else
                {
                    Logger.Log("SaveCart - Error", BRBFunctions_CSharp.iErrMsg);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("PayCart", ex);
            }
            //    sbCart.Append("<properties>");

            //    foreach (DataRow dataRow in iCartTbl.Rows)
            //    {
            //        sbCart.Append("<property>");
            //        sbCart.AppendFormat("<propno>{0}</propno>", dataRow.Field<string>("PropertyID"));
            //        sbCart.AppendFormat("<addr>{0}</addr>", dataRow.Field<string>("MainAddr"));
            //        sbCart.AppendFormat("<cfees>{0}</cfees>", dataRow.Field<decimal>("CurrFees"));
            //        sbCart.AppendFormat("<pfees>{0}</pfees>", dataRow.Field<decimal>("PriorFees"));
            //        sbCart.AppendFormat("<cpen>{0}</cpen>", dataRow.Field<decimal>("CurrPenalty"));
            //        sbCart.AppendFormat("<ppen>{0}</ppen>", dataRow.Field<decimal>("PriorPenalty"));
            //        sbCart.AppendFormat("<creds>{0}</creds>", dataRow.Field<decimal>("Credits"));
            //        sbCart.AppendFormat("<bal>{0}</bal>", dataRow.Field<decimal>("Balance"));
            //        sbCart.Append("</property>");
            //    }

            //    sbCart.Append("</properties>");

            // TODO: Build XML to send to ACI Universal Payment
        }

        protected void EditCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditCart");
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
                var value = 0.0M;
                var colIndex = e.Row.GetColumnIndexByName("Balance");
                string txtBalance = e.Row.Cells[colIndex].Text.Replace("$", "");
                Decimal.TryParse(txtBalance, out value);
                totalBalance += value;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = totalBalance.ToString("c");
            }
        }
    }
}