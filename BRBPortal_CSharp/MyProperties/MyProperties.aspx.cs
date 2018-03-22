using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyProperties : System.Web.UI.Page
    {
        //public int iNumProps;
        //public string iCartStr = "";
        public DataTable iCartTbl = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                var userCode = Session["UserCode"] as String ?? "";
                var billingCode = Session["BillingCode"] as String ?? "";

                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
                {
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    if (BRBFunctions_CSharp.GetUserProperties(userCode, billingCode))
                    {
                        // Fill table on screen from iPropertyTbl
                        //iNumProps = BRBFunctions_CSharp.iPropertyTbl.Rows.Count;
                        gvProperties.DataSource = BRBFunctions_CSharp.iPropertyTbl;
                        gvProperties.DataBind();
                        gvProperties.Columns[1].Visible = false;

                        Session["PropertyTbl"] = BRBFunctions_CSharp.iPropertyTbl;
                    }
                }
            }
        }

        //protected void AddCart_Click(object sender, EventArgs e)
        //{
        //    var rowFound = false;
        //    var tPropNo = "";
        //    var sbCart = new StringBuilder();

        //    if (iCartTbl != null || iCartTbl.Columns.Count < 1)
        //    {
        //        iCartTbl.Columns.Add("PropertyID", typeof(String));
        //        iCartTbl.Columns.Add("MainAddr", typeof(String));
        //        iCartTbl.Columns.Add("CurrFees", typeof(Decimal));
        //        iCartTbl.Columns.Add("PriorFees", typeof(Decimal));
        //        iCartTbl.Columns.Add("CurrPenalty", typeof(Decimal));
        //        iCartTbl.Columns.Add("PriorPenalty", typeof(Decimal));
        //        iCartTbl.Columns.Add("Credits", typeof(Decimal));
        //        iCartTbl.Columns.Add("Balance", typeof(Decimal));
        //    }

        //    if (Session["Cart"] != null && Session["Cart"].ToString() != "")
        //    {
        //        PopulateCartView();
        //    }

        //    foreach (GridViewRow row in gvProperties.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            CheckBox chkRow = row.Cells[0].FindControl("chkProp") as CheckBox;

        //            if (chkRow.Checked)
        //            {
        //                tPropNo = row.Cells[1].Text;
        //                foreach (DataRow dataRow in iCartTbl.Rows)
        //                {
        //                    if (dataRow.Field<string>("PropertyID") == tPropNo)
        //                    {
        //                        rowFound = true;
        //                        dataRow.SetField<string>("MainAddr", row.Cells[2].Text);
        //                        dataRow.SetField<decimal>("CurrFees", GetRowDecimalValue(row, cellIndex: 3));
        //                        dataRow.SetField<decimal>("PriorFees", GetRowDecimalValue(row, cellIndex: 4));
        //                        dataRow.SetField<decimal>("CurrPenalty", GetRowDecimalValue(row, cellIndex: 5));
        //                        dataRow.SetField<decimal>("PriorPenalty", GetRowDecimalValue(row, cellIndex: 6));
        //                        dataRow.SetField<decimal>("Credits", GetRowDecimalValue(row, cellIndex: 7));
        //                        dataRow.SetField<decimal>("Balance", GetRowDecimalValue(row, cellIndex: 8));
        //                    }
        //                }

        //                if (rowFound == false)
        //                {
        //                    DataRow NR = iCartTbl.NewRow();
        //                    NR.SetField<string>("PropertyID", tPropNo);
        //                    NR.SetField<string>("MainAddr", row.Cells[2].Text);
        //                    NR.SetField<decimal>("CurrFees", GetRowDecimalValue(row, cellIndex: 3));
        //                    NR.SetField<decimal>("PriorFees", GetRowDecimalValue(row, cellIndex: 4));
        //                    NR.SetField<decimal>("CurrPenalty", GetRowDecimalValue(row, cellIndex: 5));
        //                    NR.SetField<decimal>("PriorPenalty", GetRowDecimalValue(row, cellIndex: 6));
        //                    NR.SetField<decimal>("Credits", GetRowDecimalValue(row, cellIndex: 7));
        //                    NR.SetField<decimal>("Balance", GetRowDecimalValue(row, cellIndex: 8));
        //                    iCartTbl.Rows.Add(NR);
        //                }
        //            }
        //        }
        //    }

        //    if (iCartTbl.Rows.Count > 0) {
        //        sbCart.Append("<properties>");

        //        foreach (DataRow dataRow in iCartTbl.Rows)
        //        {
        //            sbCart.Append("<property>");
        //            sbCart.AppendFormat("<propno>{0}</propno>", dataRow.Field<string>("PropertyID"));
        //            sbCart.AppendFormat("<addr>{0}</addr>", dataRow.Field<string>("MainAddr"));
        //            sbCart.AppendFormat("<cfees>{0}</cfees>", dataRow.Field<decimal>("CurrFees"));
        //            sbCart.AppendFormat("<pfees>{0}</pfees>", dataRow.Field<decimal>("PriorFees"));
        //            sbCart.AppendFormat("<cpen>{0}</cpen>", dataRow.Field<decimal>("CurrPenalty"));
        //            sbCart.AppendFormat("<ppen>{0}</ppen>", dataRow.Field<decimal>("PriorPenalty"));
        //            sbCart.AppendFormat("<creds>{0}</creds>", dataRow.Field<decimal>("Credits"));
        //            sbCart.AppendFormat("<bal>{0}</bal>", dataRow.Field<decimal>("Balance"));
        //            sbCart.Append("</property>");
        //        }

        //        sbCart.Append("</properties>");
        //    }

        //    iCartStr = sbCart.ToString();
        //    Session["Cart"] = iCartStr;

        //    // Loop through and uncheck all rows
        //    foreach (GridViewRow row in gvProperties.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            var chkRow = row.Cells[0].FindControl("chkProp") as CheckBox;

        //            if (chkRow.Checked) {
        //                chkRow.Checked = false;
        //            }
        //        }
        //    }
        //}

        private decimal GetRowDecimalValue(GridViewRow row, int cellIndex, decimal defaultValue = 0.0M )
        {
            var value = defaultValue;

            Decimal.TryParse(row.Cells[cellIndex].Text, out value);

            return value;
        }

        protected void UpdatePropClicked(object sender, GridViewCommandEventArgs e)
        {
            var RowNum = Convert.ToInt32(e.CommandArgument);

            Session["PropertyID"] = BRBFunctions_CSharp.iPropertyTbl.Rows[RowNum].Field<string>("PropertyID");
            Session["PropAddr"] = BRBFunctions_CSharp.iPropertyTbl.Rows[RowNum].Field<string>("MainAddr");
            Session["PropBalance"] = BRBFunctions_CSharp.iPropertyTbl.Rows[RowNum].Field<decimal>("Balance").ToString();

            Response.Redirect("~/MyProperties/MyUnits");
        }

        protected void gvProperties_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProperties.PageIndex = e.NewPageIndex;
            gvProperties.DataSource = Session["PropertyTbl"];
            gvProperties.DataBind();
        }

        //private void PopulateCartView()
        //{
        //    if (Session["Cart"] != null && Session["Cart"].ToString() != "")
        //    {
        //        var xmlDoc = new XmlDocument();

        //        xmlDoc.LoadXml(Session["Cart"].ToString());

        //        foreach (XmlElement detail in xmlDoc.DocumentElement.GetElementsByTagName("property"))
        //        {
        //            decimal currentFees = 0;
        //            decimal priorFees = 0;
        //            decimal currentPenalty = 0;
        //            decimal priorPenalty = 0;
        //            decimal credit = 0;
        //            decimal totalBalance = 0;

        //            Decimal.TryParse(detail.SelectSingleNode("currentFees").InnerText, out currentFees);
        //            Decimal.TryParse(detail.SelectSingleNode("priorFees").InnerText, out priorFees);
        //            Decimal.TryParse(detail.SelectSingleNode("currentPenalty").InnerText, out currentPenalty);
        //            Decimal.TryParse(detail.SelectSingleNode("priorPenalties").InnerText, out priorPenalty);
        //            Decimal.TryParse(detail.SelectSingleNode("credit").InnerText, out credit);
        //            Decimal.TryParse(detail.SelectSingleNode("totalBalance").InnerText, out totalBalance);

        //            DataRow NR = iCartTbl.NewRow();
        //            NR.SetField<string>("PropertyID", detail.SelectSingleNode("propno").InnerText);
        //            NR.SetField<string>("MainAddr", detail.SelectSingleNode("addr").InnerText);
        //            NR.SetField<Decimal>("CurrFees", currentFees);
        //            NR.SetField<Decimal>("PriorFees", priorFees);
        //            NR.SetField<Decimal>("CurrPenalty", currentPenalty);
        //            NR.SetField<Decimal>("PriorPenalty", priorPenalty);
        //            NR.SetField<Decimal>("Credits", credit);
        //            NR.SetField<Decimal>("Balance", totalBalance);
        //            iCartTbl.Rows.Add(NR);
        //        }
        //    }
        //}
    }
}