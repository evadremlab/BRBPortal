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
        public Decimal iBalance = 0.0M;
        public DataTable iCartTbl = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                if (iCartTbl != null || iCartTbl.Columns.Count < 1) {
                    iCartTbl.Columns.Add("PropertyID", typeof(String));
                    iCartTbl.Columns.Add("MainAddr", typeof(String));
                    iCartTbl.Columns.Add("CurrFees", typeof(Decimal));
                    iCartTbl.Columns.Add("PriorFees", typeof(Decimal));
                    iCartTbl.Columns.Add("CurrPenalty", typeof(Decimal));
                    iCartTbl.Columns.Add("PriorPenalty", typeof(Decimal));
                    iCartTbl.Columns.Add("Credits", typeof(Decimal));
                    iCartTbl.Columns.Add("Balance", typeof(Decimal));
                }

                btnEdCart.Enabled = false;
                btnPayCart.Enabled = false;

                if (Session["Cart"] != null && Session["Cart"].ToString() != "")
                {
                    PopulateCartView();

                    gvCart.DataSource = iCartTbl;
                    gvCart.DataBind();
                    gvCart.Columns[0].Visible = false; // Hide the PropertyID column

                    Session["CartTbl"] = iCartTbl;

                    btnEdCart.Enabled = true;
                    btnPayCart.Enabled = true;

                    if (Session["FeesAll"] != null) {
                        ShowFeesAll.Text = "All Fees and Penalties";
                    } else {
                        if (Session["FeesAll"].ToString() == "AllFees" || Session["FeesAll"].ToString() == "") {
                            ShowFeesAll.Text = "All Fees and Penalties";
                        } else {
                            ShowFeesAll.Text = "Fees Only";
                        }
                    }
                } else {
                    ShowFeesAll.Text = "Nothing in your cart.";
                }
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

            PopulateCartView();

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
                string txtBalance = e.Row.Cells[7].Text;
                Decimal.TryParse(txtBalance, out value);
                iBalance += value;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = iBalance.ToString("c");
            }
        }

        private void PopulateCartView()
        {
            if (Session["Cart"] != null && Session["Cart"].ToString() != "")
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(Session["Cart"].ToString());

                foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("property"))
                {
                    decimal currentFees = 0;
                    decimal priorFees = 0;
                    decimal currentPenalty = 0;
                    decimal priorPenalty = 0;
                    decimal credit = 0;
                    decimal totalBalance = 0;

                    Decimal.TryParse(detail.SelectSingleNode("currentFees").InnerText, out currentFees);
                    Decimal.TryParse(detail.SelectSingleNode("priorFees").InnerText, out priorFees);
                    Decimal.TryParse(detail.SelectSingleNode("currentPenalty").InnerText, out currentPenalty);
                    Decimal.TryParse(detail.SelectSingleNode("priorPenalties").InnerText, out priorPenalty);
                    Decimal.TryParse(detail.SelectSingleNode("credit").InnerText, out credit);
                    Decimal.TryParse(detail.SelectSingleNode("totalBalance").InnerText, out totalBalance);

                    DataRow NR = iCartTbl.NewRow();
                    NR.SetField<string>("PropertyID", detail.SelectSingleNode("propno").InnerText);
                    NR.SetField<string>("MainAddr", detail.SelectSingleNode("addr").InnerText);
                    NR.SetField<Decimal>("CurrFees", currentFees);
                    NR.SetField<Decimal>("PriorFees", priorFees);
                    NR.SetField<Decimal>("CurrPenalty", currentPenalty);
                    NR.SetField<Decimal>("PriorPenalty", priorPenalty);
                    NR.SetField<Decimal>("Credits", credit);
                    NR.SetField<Decimal>("Balance", totalBalance);
                    iCartTbl.Rows.Add(NR);
                }
            }
        }
    }
}