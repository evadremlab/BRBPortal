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
                    btnEdCart.Enabled = false;
                    btnPayCart.Enabled = false;
                    ShowFeesAll.Text = "Nothing in your cart.";
                    return;
                }

                if (!IsPostBack)
                {
                    gvCart.DataSource = iCartTbl;
                    gvCart.DataBind();
                    gvCart.Columns[0].Visible = false; // Hide the PropertyID column

                    btnEdCart.Enabled = true;
                    btnPayCart.Enabled = true;

                    if (Session["FeesAll"] == null)
                    {
                        ShowFeesAll.Text = "All Fees and Penalties";
                    }
                    else
                    {
                        if (Session["FeesAll"].ToString() == "AllFees" || Session["FeesAll"].ToString() == "")
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
                ShowDialogOK(ex.Message, "Cart View");
            }
        }

        protected void CancelCart_Click(object sender, EventArgs e)
        {
            Session["FeesAll"] = "AllFees";
            Response.Redirect("~/MyProperties/MyProperties");
        }

        protected void PayCart_Click(object sender, EventArgs e)
        {
            var XMLstr = "";
            decimal tSubTotal = 0.0M;

            // TODO: Build XML to send to ACI Universal Payment
        }

        protected void EditCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditCart");
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

        protected void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}