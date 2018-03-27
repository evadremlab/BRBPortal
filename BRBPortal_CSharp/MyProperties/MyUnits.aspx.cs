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
    public partial class MyUnits : System.Web.UI.Page
    {
        private string iPropertyNo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                string userCode = Master.User.UserCode;
                string billingCode = Master.User.BillingCode;
                var currentFees = Session["CurrFees"] as String ?? "0";
                var propertyBalance = Session["PropBalance"] as String ?? "0";
                var propertyID = Session["PropertyID"] as String ?? "";
                var propertyAddress = Session["PropAddr"] as String ?? "";

                var user = Master.User;
                BRBFunctions_CSharp.GetPropertyUnits(ref user, propertyID);

                if (!string.IsNullOrEmpty(BRBFunctions_CSharp.iErrMsg))
                {
                    if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                    {
                        BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                    }
                    ShowDialogOK("Error retrieving Units: " + BRBFunctions_CSharp.iErrMsg, "View Units");
                    return;
                }

                Master.UpdateSession(user);

                if (user.CurrentProperty.Units.Count == 0)
                {
                    ShowDialogOK("No Units found for this property.", "View Units");
                    return;
                }

                //BRBFunctions_CSharp.iUnitsTbl = BRBFunctions_CSharp.ConvertToDataTable(propertyUnits);
                //BRBFunctions_CSharp.iUnitsTbl.DefaultView.Sort = "UnitNo ASC";

                //gvUnits.DataSource = BRBFunctions_CSharp.iUnitsTbl;
                //gvUnits.DataBind();

                gvUnits.Columns[2].Visible = false; // Do this so it stores the value in the GV but doesn't show it

                CurrFee.Text = currentFees;
                Balance.Text = propertyBalance;
                PropAddr.Text = propertyAddress;
                MainAddress.Text = user.CurrentProperty.MainStreetAddress;
                BillAddr.Text = user.CurrentProperty.BillingAddress;
                MgrName.Text = user.AgencyName;

                AgentSection.Visible = !string.IsNullOrEmpty(MgrName.Text);

                Session["UnitsTbl"] = BRBFunctions_CSharp.iUnitsTbl;
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void ToProperty_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyProperties", false);
        }

        //protected void RemAgent_Click(object sender, EventArgs e)
        //{
        //    ShowDialogOK("Please call the Rent Board to remove an agent.", "List Units");
        //}

        protected void gvUnits_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUnits.PageIndex = e.NewPageIndex;
            gvUnits.DataSource = Session["PropertyTbl"];
            gvUnits.DataBind();

            gvUnits.Columns[2].Visible = false; // Do this so it stores the value in the GV but doesn't show it
        }

        protected void gvUnits_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvUnits.Rows[rowIndex];

            var tUnitID = row.Cells[0].Text; // Unit ID
            var tUnit = row.Cells[1].Text; // Unit Number

            Session["UnitNo"] = tUnit;
            Session["UnitID"] = tUnitID;
            Session["UpdTenants"] = false;

            if (e.CommandName.Equals("Tenancy"))
            {
                Response.Redirect("~/MyProperties/MyTenants");
            }
            else
            {
                //// Build XML string for the Unit page NOT USED
                //var tUnitStr = iPropertyNo + tsep + Session["PropAddr"].ToString() + tsep + tUnit + tsep;
                //tUnitStr += row.Cells[5].Text + tsep; //  Unit Status
                //Session["UpdUnitInfo"] = tUnitStr;

                Response.Redirect("~/MyProperties/UpdateUnit");
            }
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}