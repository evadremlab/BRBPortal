using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class UpdateTenancy : System.Web.UI.Page
    {
        public List<BRBTenant> Tenants = null;
        public string DelimitedTenants = ""; // updated when tenants edited client-side
        public string RemovedTenantIDs = ""; // updated when existing tenants Removed client-side

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var user = Master.User;
                var property = user.CurrentProperty;
                var unit = user.CurrentUnit;

                this.Tenants = unit.Tenants;

                if (IsPostBack)
                {
                    btnSubmit.Attributes.Remove("disabled"); // enable button after postback, in case of errors
                    return;
                }

                btnSubmit.Attributes.Add("disabled", "disabled");
                ExplainOtherTermination.Style.Add("display", "none");

                // Literals
                MainAddress.Text = property.PropertyAddress;
                UnitNo.Text = unit.UnitNo;
                OwnerName.Text = property.OwnerContactName;
                AgentName.Text = property.AgencyName;
                BalAmt.Text = property.Balance.ToString("C");
                UnitStatus.Text = unit.ClientPortalUnitStatusCode;

                // Fields
                InitRent.Text = unit.InitialRent;

                if (unit.TenancyStartDate.HasValue)
                {
                    TenStDt.Text = unit.TenancyStartDate.Value.ConvertForDatePicker();
                }

                var otherServices = new List<string>();
                OtherHousingServices.Style.Add("display", "none");
                var housingServices = unit.HServices.Split(',').ToList<string>();
                foreach (var svc in housingServices)
                {
                    var service = svc.Trim();
                    var item = HServs.Items.FindByText(service);

                    if (item == null)
                    {
                        otherServices.Add(service);
                        OtherHousingServices.Style.Remove("display");
                        HServs.Items.FindByText("Other").Selected = true;
                    }
                    else
                    {
                        item.Selected = true;
                    }
                }
                HServOthrBox.Text = string.Join(", ", otherServices);

                RB1.SelectedValue = unit.SmokingProhibitionInLeaseStatus;

                if (unit.SmokingProhibitionEffectiveDate.HasValue)
                {
                    SmokeDt.Text = unit.SmokingProhibitionEffectiveDate.Value.ConvertForDatePicker();
                }

                if (unit.DatePriorTenancyEnded.HasValue)
                {
                    PTenDt.Text = unit.DatePriorTenancyEnded.Value.ConvertForDatePicker();
                }

                if (!string.IsNullOrEmpty(unit.ReasonPriorTenancyEnded))
                {
                    var item = TermReas.Items.FindByText(unit.ReasonPriorTenancyEnded);

                    if (item == null)
                    {
                        TermReas.SelectedValue = "4"; // Other
                        TermDescr.Text = unit.ReasonPriorTenancyEnded;
                    }
                    else
                    {
                        ExplainOtherTermination.Style.Add("display", "none");
                        TermReas.SelectedValue = unit.ReasonPriorTenancyEnded;
                    }
                }

                if (string.IsNullOrEmpty(AgentName.Text))
                {
                    AgencyNameSection.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("UpdateTenancy - Page_Load()", ex);
                Master.ShowErrorModal("Error loading Tenancy details.");
            }
        }

        protected void UpdateTenancy_Click(object sender, EventArgs e)
        {
            var user = Master.User;
            var unit = user.CurrentUnit;
            var tenants = unit.Tenants;

            try
            {
                foreach (var id in hdnRemovedTenantIDs.Value.Split(','))
                {
                    var existingTenant = unit.Tenants.Where(x => x.TenantID == id).SingleOrDefault();

                    if (existingTenant != null)
                    {
                        unit.Tenants.Remove(existingTenant);
                    }
                }

                foreach (var str in hdnDelimitedTenants.Value.Split('|'))
                {
                    var fields = str.Split('^');

                    if (fields.Length > 1) // ignore "no tenants" entry
                    {
                        var tenant = new BRBTenant
                        {
                            TenantID = fields[0],
                            FirstName = fields[1],
                            LastName = fields[2],
                            PhoneNumber = fields[3],
                            Email = fields[4]
                        };

                        if (tenant.TenantID == "-1") // new Tenant
                        {
                            unit.Tenants.Add(tenant);
                        }
                    }
                }

                unit.InitialRent = InitRent.Text;

                if (!string.IsNullOrEmpty(TenStDt.Text))
                {
                    unit.TenancyStartDate = DateTime.Parse(TenStDt.Text);
                }

                var housingServices = new List<string>();

                foreach (ListItem item in HServs.Items)
                {
                    if (item.Selected && item.Text != "Other")
                    {
                        housingServices.Add(item.Text);
                    }
                }

                unit.HServices = string.Join(",", housingServices);

                if (!string.IsNullOrEmpty(HServOthrBox.Text))
                {
                    unit.OtherHServices = HServOthrBox.Text;
                }

                unit.SmokingProhibitionInLeaseStatus = RB1.SelectedValue;

                if (!string.IsNullOrEmpty(SmokeDt.Text))
                {
                    unit.SmokingProhibitionEffectiveDate = DateTime.Parse(SmokeDt.Text);
                }

                if (!string.IsNullOrEmpty(PTenDt.Text))
                {
                    unit.DatePriorTenancyEnded = DateTime.Parse(PTenDt.Text);
                }

                unit.TerminationReason = TermReas.SelectedValue;
                unit.OtherTerminationReason = TermDescr.Text;

                unit.DeclarationInitials = DeclareInits.Text;

                if (Master.DataProvider.UpdateUnitTenancy(ref user))
                {
                    Session["ShowAfterRedirect"] = "Tenancy has been updated.|Update Tenancy";

                    Response.Redirect("~/MyProperties/MyUnits", false);
                }
                else
                {
                    Master.ShowErrorModal(Master.DataProvider.ErrorMessage, "Update Tenancy");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("UpdateTenancy", ex);
                Master.ShowErrorModal("Error updating Tenancy.");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyUnits");
        }
    }
}