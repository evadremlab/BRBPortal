using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyUnits : System.Web.UI.Page
    {
        private string iPropertyAddress = "";
        private string iPropertyNo = "";

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
                var propertyID = Session["PropertyID"] as String ?? "";
                var propertyAddress = Session["PropAddr"] as String ?? "";

                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
                {
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    var propertyUnits = BRBFunctions_CSharp.GetPropertyUnits(propertyID, userCode, billingCode);

                    if (propertyUnits == "FAILURE")
                    {
                        if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                        {
                            BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                        }
                        ShowDialogOK("Error retrieving Units: " + BRBFunctions_CSharp.iErrMsg, "View Units");
                        return;
                    }

                    BRBFunctions_CSharp.iUnitsTbl.DefaultView.Sort = "UnitNo ASC";
                    BRBFunctions_CSharp.iUnitsTbl = BRBFunctions_CSharp.iUnitsTbl.DefaultView.ToTable();

                    gvUnits.DataSource = BRBFunctions_CSharp.iUnitsTbl;
                    gvUnits.DataBind();

                    gvUnits.Columns[2].Visible = false; // Do this so it stores the value in the GV but doesn't show it

                    MainAddress.Text = BRBFunctions_CSharp.iPropAddr;
                    BillAddr.Text = BRBFunctions_CSharp.iBillAddr;
                    MgrName.Text = BRBFunctions_CSharp.iAgentName;

                    Session["UnitsTbl"] = BRBFunctions_CSharp.iUnitsTbl;
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void NextBtn_Click(object sender, EventArgs e)
        {
            var tUnit = "";
            var tUnitStr = "";
            var tUnitID = "";
            var tsep = "::";

            if (gvUnits.Rows.Count > 0)
            {
                foreach (GridViewRow row in gvUnits.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        tUnitID = row.Cells[2].Text; // Unit ID
                        tUnit = row.Cells[3].Text; // Unit Number
                        Session["UpdTenants"] = false;

                        CheckBox chkUnitStatus = row.Cells[0].Controls[1] as CheckBox; // Unit Status
                        CheckBox chkTenancyUpdate = row.Cells[1].Controls[1] as CheckBox; // Tenancy Update

                        if (chkUnitStatus.Checked)
                        {
                            // Check if the Tenancy Update was also checked
                            if (chkTenancyUpdate.Checked)
                            {
                                Session["UpdTenants"] = true;
                            }

                            Session["UnitNo"] = tUnit;
                            Session["UnitID"] = tUnitID;

                            // Build XML string for the Unit page
                            tUnitStr = iPropertyNo + tsep + Session["PropAddr"].ToString() + tsep + tUnit + tsep;
                            tUnitStr += row.Cells[5].Text + tsep; //  Unit Status
                            Session["UpdUnitInfo"] = tUnitStr;

                            Response.Redirect("~/MyProperties/UpdateUnit");
                            break;
                        }

                        if (chkTenancyUpdate.Checked)
                        {
                            Session["UnitNo"] = tUnit;
                            Session["UnitID"] = tUnitID;
                            Response.Redirect("~/MyProperties/MyTenants");
                            break;
                        }
                    }
                }
            }
        }

        protected void ToProperty_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyProperties", false);
        }

        protected void RemAgent_Click(object sender, EventArgs e)
        {
            ShowDialogOK("Please call the Rent Board to remove an agent.", "List Units");
        }

        protected void gvUnits_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUnits.PageIndex = e.NewPageIndex;
            gvUnits.DataSource = Session["PropertyTbl"];
            gvUnits.DataBind();

            gvUnits.Columns[2].Visible = false; // Do this so it stores the value in the GV but doesn't show it
        }

        protected void cbUnit_CheckedChanged(object sender, EventArgs e)
        {
            var tChkBox = (CheckBox)sender;
            var tRow = (GridViewRow)tChkBox.Parent.Parent;

            foreach (GridViewRow row in gvUnits.Rows)
            {
                CheckBox tOthrChkBox = row.Cells[0].Controls[1] as CheckBox; // Unit Status

                if (row.RowIndex == tRow.RowIndex)
                {
                    continue;
                }

                if (tOthrChkBox.Checked)
                {
                    tOthrChkBox.Checked = false;
                }
            }
        }

        protected void cbTenant_CheckedChanged(object sender, EventArgs e)
        {
            var tChkBox = (CheckBox)sender;
            var tRow = (GridViewRow)tChkBox.Parent.Parent;

            foreach (GridViewRow row in gvUnits.Rows)
            {
                CheckBox tOthrChkBox = row.Cells[1].Controls[1] as CheckBox; // Tenancy Update

                if (row.RowIndex == tRow.RowIndex)
                {
                    continue;
                }

                if (tOthrChkBox.Checked)
                {
                    tOthrChkBox.Checked = false;
                }
            }
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}