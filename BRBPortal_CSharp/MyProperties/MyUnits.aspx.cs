using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using BRBPortal_CSharp.Models;
using System.Text.RegularExpressions;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyUnits : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = Master.User;

                if (BRBFunctions_CSharp.GetPropertyUnits(ref user))
                {
                    var units = user.CurrentProperty.Units;

                    Master.UpdateSession(user);

                    if (units.Count == 0)
                    {
                        units.Add(new BRBUnit
                        {
                            StreetAddress = "no units"
                        });
                    }

                    var dataTable = BRBFunctions_CSharp.ConvertToDataTable<BRBUnit>(units);
                    dataTable.DefaultView.Sort = "UnitNo ASC";
                    gvUnits.DataSource = dataTable;
                    gvUnits.DataBind();

                    CurrFee.Text = user.CurrentProperty.CurrentFee.ToString("C");
                    Balance.Text = user.CurrentProperty.Balance.ToString("C");
                    PropertyAddress.Text = user.CurrentProperty.PropertyAddress;
                    BillingAddress.Text = user.CurrentProperty.BillingAddress;
                    MgrName.Text = user.AgencyName;

                    AgentSection.Visible = !string.IsNullOrEmpty(MgrName.Text);

                    var totalRented = 0;
                    var totalExempt = 0;

                    foreach (var unit in units)
                    {
                        if (unit.ClientPortalUnitStatusCode == "Rented")
                        {
                            totalRented++;
                        }
                        else if (unit.ClientPortalUnitStatusCode == "Rented")
                        {
                            totalExempt++;
                        }
                    }
                    UnitStatusDescription.Text = string.Format("Out of {0} units for this property, you have {1} units in Rented and {2} in Exempt", totalRented + totalExempt, totalRented, totalExempt);
                }
                else
                {
                    if (!string.IsNullOrEmpty(BRBFunctions_CSharp.iErrMsg))
                    {
                        if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                        {
                            BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                        }

                        Master.ShowErrorModal("Error retrieving Units: " + BRBFunctions_CSharp.iErrMsg, "Units");
                        return;
                    }
                }
            }
        }

        protected void gvUnits_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var updateTenancyButton = e.Row.Cells[10];
                var unitStatus = e.Row.Cells[e.Row.GetColumnIndexByName("ClientPortalUnitStatusCode")].Text;

                updateTenancyButton.Enabled = Regex.IsMatch(unitStatus, "Rented", RegexOptions.IgnoreCase);
            }
        }

        protected void gvUnits_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var user = Master.User;

            gvUnits.PageIndex = e.NewPageIndex;
            gvUnits.DataSource = BRBFunctions_CSharp.ConvertToDataTable<BRBUnit>(user.CurrentProperty.Units);
            gvUnits.DataBind();

            gvUnits.Columns[2].Visible = false; // Do this so it stores the value in the GV but doesn't show it
        }

        protected void gvUnits_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            var user = Master.User;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvUnits.Rows[rowIndex];

            user.CurrentUnit = user.CurrentProperty.Units[rowIndex];

            Master.UpdateSession(user);

            if (e.CommandName.Equals("Tenancy"))
            {
                Response.Redirect("~/MyProperties/MyTenants", false);
            }
            else
            {
                Response.Redirect("~/MyProperties/UpdateUnit", false);
            }
        }

        protected void AddCart_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var user = Master.User;
                var newCartItem = user.CurrentProperty.ConvertToCartItem();

                user.Cart.Items.RemoveAll(x => x.PropertyId == newCartItem.PropertyId);

                user.Cart.Items.Add(newCartItem);
                user.Cart.WasUpdated = true;

                Master.UpdateSession(user);

                btnAddCart.Enabled = false;

                Response.Redirect("~/Cart", false);
            }
        }
    }
}