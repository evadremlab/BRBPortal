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
        public Decimal iBalance = 0.0M;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cart"] == null)
                {
                    iCartTbl = new DataTable();
                    Session["Cart"] = iCartTbl;
                }
                else
                {
                    iCartTbl = (DataTable)Session["Cart"];
                }

                if (iCartTbl.Columns.Count < 1)
                {
                    iCartTbl.Columns.Add("PropertyID", typeof(String));
                    iCartTbl.Columns.Add("MainAddr", typeof(String));
                    iCartTbl.Columns.Add("CurrFees", typeof(Decimal));
                    iCartTbl.Columns.Add("PriorFees", typeof(Decimal));
                    iCartTbl.Columns.Add("CurrPenalty", typeof(Decimal));
                    iCartTbl.Columns.Add("PriorPenalty", typeof(Decimal));
                    iCartTbl.Columns.Add("Credits", typeof(Decimal));
                    iCartTbl.Columns.Add("Balance", typeof(Decimal));
                }

                if (iCartTbl.Rows.Count == 0)
                {
                    Response.Redirect("~/Cart.aspx", true);
                    return;
                }

                if (!IsPostBack)
                {
                    gvCart.DataSource = iCartTbl;
                    gvCart.DataBind();
                    gvCart.Columns[0].Visible = false; // Hide the PropertyID column
                }
            }
            catch (Exception ex)
            {
                ShowDialogOK(ex.Message, "Edit Cart View");
            }
        }

        protected void UpdateCart_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/EditCart");
        }

        protected void gvCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCart.PageIndex = e.NewPageIndex;
            gvCart.DataSource = Session["CartTbl"];
            gvCart.DataBind();
        }

        protected void gvCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var value = 0.0M;
                string txtBalance = e.Row.Cells[7].Text.Replace("$", "");
                Decimal.TryParse(txtBalance, out value);
                iBalance += value;
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
                    var gridRow = iCartTbl.Rows[rowIndex];

                    if (gridRow == null)
                    {
                        ShowDialogOK("Matching row not found in Session[CartTbl]", "Remove cart item");
                    }
                    else
                    {
                        gridRow.Delete();
                        iCartTbl.AcceptChanges();
                        Session["Cart"] = iCartTbl;

                        if (iCartTbl.Rows.Count == 0)
                        {
                            Session["ShowFeesAll"] = "All items removed from your cart.";
                        }
                        else
                        {
                            Session.Remove("ShowFeesAll");
                        }

                        Response.Redirect("~/EditCart.aspx", true);
                    }
                }
                catch (Exception ex)
                {
                    ShowDialogOK("Error removing cart item: " + ex.Message, "Remove cart item");
                }
            }
        }

        protected void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}