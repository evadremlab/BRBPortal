﻿using System;
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
    public partial class Cart : System.Web.UI.Page
    {
        public Decimal totalBalance;

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

                // TODO: what if they already have a Cart ID?

                if (BRBFunctions_CSharp.SaveCart(user))
                {
                    if (user.Cart.ID.HasValue)
                    {
                        Master.UpdateSession(user);
                        Response.Redirect("~/ConfirmPayment");
                    }
                    else
                    {
                        Master.ShowDialogOK("Error saving cart(1)", "Save Cart");
                    }
                }
                else
                {
                    Master.ShowDialogOK("Error saving cart(2).", "Save Cart");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("PayCart", ex);
            }
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