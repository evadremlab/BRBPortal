using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyUnits : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["BackFromCartUrl"] = "~/MyProperties/MyUnits";

            if (!IsPostBack)
            {
                var user = Master.User;
                var provider = Master.DataProvider;

                if (provider.GetPropertyUnits(ref user))
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

                    var dataTable = provider.ConvertToDataTable<BRBUnit>(units);
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
                    Master.ShowErrorModal("Error retrieving Units: " + provider.ErrorMessage, "Units");
                    return;
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
            var provider = Master.DataProvider;

            gvUnits.PageIndex = e.NewPageIndex;
            gvUnits.DataSource = provider.ConvertToDataTable<BRBUnit>(user.CurrentProperty.Units);
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