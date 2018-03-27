using BRBPortal_CSharp.Models;
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
        public DataTable iCartTbl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                var user = Master.User;

                if (BRBFunctions_CSharp.GetUserProperties(ref user))
                {
                    // Fill table on screen from iPropertyTbl
                    gvProperties.DataSource = BRBFunctions_CSharp.iPropertyTbl;
                    gvProperties.DataBind();
                    gvProperties.Columns[1].Visible = false;

                    Session["PropertyTbl"] = BRBFunctions_CSharp.iPropertyTbl;

                    Master.UpdateSession(user);
                }
            }
        }

        protected void gvProperties_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvProperties.Rows[rowIndex];

            Session["PropertyID"] = BRBFunctions_CSharp.iPropertyTbl.Rows[rowIndex].Field<string>("PropertyID");
            Session["PropAddr"] = BRBFunctions_CSharp.iPropertyTbl.Rows[rowIndex].Field<string>("MainAddr");
            Session["CurrFees"] = BRBFunctions_CSharp.iPropertyTbl.Rows[rowIndex].Field<decimal>("CurrFees").ToString("C");
            Session["PropBalance"] = BRBFunctions_CSharp.iPropertyTbl.Rows[rowIndex].Field<decimal>("Balance").ToString("C");

            var currentProperty = new BRBProperty
            {
                PropertyID = BRBFunctions_CSharp.iPropertyTbl.Rows[rowIndex].Field<string>("PropertyID"),
                CurrentFee = BRBFunctions_CSharp.iPropertyTbl.Rows[rowIndex].Field<decimal>("CurrFees"),
            };

            if (e.CommandName.Equals("Select"))
            {
                Session["CurrentFees"] = "";
                Response.Redirect("~/MyProperties/MyUnits");
            }
        }

        protected void AddToCart(GridViewRow row)
        {
            var rowFound = false;
            var tPropNo = row.Cells[1].Text;

            //if (Session["Cart"] == null)
            //{
            //    iCartTbl = new DataTable();
            //}
            //else
            //{
            //    iCartTbl = (DataTable)Session["Cart"];
            //}

            //AddToCart(row);

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

            foreach (DataRow dataRow in iCartTbl.Rows)
            {
                if (dataRow.Field<string>("PropertyID") == tPropNo)
                {
                    rowFound = true;
                    dataRow.SetField<string>("MainAddr", row.Cells[2].Text);
                    dataRow.SetField<decimal>("CurrFees", GetRowDecimalValue(row, cellIndex: 3));
                    dataRow.SetField<decimal>("PriorFees", GetRowDecimalValue(row, cellIndex: 4));
                    dataRow.SetField<decimal>("CurrPenalty", GetRowDecimalValue(row, cellIndex: 5));
                    dataRow.SetField<decimal>("PriorPenalty", GetRowDecimalValue(row, cellIndex: 6));
                    dataRow.SetField<decimal>("Credits", GetRowDecimalValue(row, cellIndex: 7));
                    dataRow.SetField<decimal>("Balance", GetRowDecimalValue(row, cellIndex: 8));
                }
            }

            if (rowFound == false)
            {
                DataRow NR = iCartTbl.NewRow();
                NR.SetField<string>("PropertyID", tPropNo);
                NR.SetField<string>("MainAddr", row.Cells[2].Text);
                NR.SetField<decimal>("CurrFees", GetRowDecimalValue(row, cellIndex: 3));
                NR.SetField<decimal>("PriorFees", GetRowDecimalValue(row, cellIndex: 4));
                NR.SetField<decimal>("CurrPenalty", GetRowDecimalValue(row, cellIndex: 5));
                NR.SetField<decimal>("PriorPenalty", GetRowDecimalValue(row, cellIndex: 6));
                NR.SetField<decimal>("Credits", GetRowDecimalValue(row, cellIndex: 7));
                NR.SetField<decimal>("Balance", GetRowDecimalValue(row, cellIndex: 8));
                iCartTbl.Rows.Add(NR);
            }

            //if (iCartTbl.Rows.Count > 0)
            //{
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
            //}

            Session["Cart"] = iCartTbl;
        }

        private decimal GetRowDecimalValue(GridViewRow row, int cellIndex, decimal defaultValue = 0.0M )
        {
            var value = defaultValue;

            Decimal.TryParse(row.Cells[cellIndex].Text.Replace("$", ""), out value);

            return value;
        }

        protected void gvProperties_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProperties.PageIndex = e.NewPageIndex;
            gvProperties.DataSource = Session["PropertyTbl"];
            gvProperties.DataBind();
        }
    }
}