using System;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class UpdateUnit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            var user = Master.User;
            var unit = user.CurrentUnit;

            hdnUnitStatus.Value = unit.ClientPortalUnitStatusCode;
            hdnExemptReas.Value = unit.UnitStatCode;

            if (!IsPostBack)
            {
                var provider = Master.DataProvider;

                btnSubmit.Attributes.Add("disabled", "disabled");
                UpdateUnitForm.Attributes.Add("class", "hidden");

                if (provider.GetPropertyUnits(ref user, user.CurrentUnit.UnitID))
                {
                    // literals
                    MainAddress.Text = unit.StreetAddress;
                    UnitNo.Text = unit.UnitNo;
                    litUnitStatus.Text = unit.ClientPortalUnitStatusCode;
                    litExemptReas.Text = unit.ExemptionReason;

                    if (string.IsNullOrEmpty(unit.ExemptionReason))
                    {
                        switch (unit.UnitStatCode)
                        {
                            case "NAR":
                                litExemptReas.Text = "Vacant and not available for rent";
                                break;
                            case "OOCC":
                                litExemptReas.Text = "Owner-Occupied";
                                break;
                            case "SEC8":
                                litExemptReas.Text = "Section 8";
                                break;
                            case "FREE":
                                litExemptReas.Text = "Occupied Rent Free";
                                break;
                            case "COMM":
                                litExemptReas.Text = "Commercial Use";
                                break;
                            case "MISC":
                                litExemptReas.Text = "Property Manager's Unit";
                                break;
                            case "SHARED":
                                litExemptReas.Text = "Owner shares kitchen & bath with tenant";
                                break;
                            case "SPLUS":
                                litExemptReas.Text = "Shelter Plus Care";
                                break;
                        }
                    }

                    if (unit.IsRented)
                    {
                        CurrentExemption.Visible = false;

                        if (unit.TenancyStartDate.HasValue)
                        {
                            litTenancyStartDate.Text = unit.TenancyStartDate.Value.ConvertForLiteral();
                            UnitAsOfDt.Text = unit.TenancyStartDate.HasValue ? unit.TenancyStartDate.Value.ConvertForDatePicker() : "";
                        }

                        litUnitOccBy.Text = unit.OccupiedBy;
                    }
                    else
                    {
                        CurrentRental.Visible = false;

                        if (unit.UnitStatusAsOfDate.HasValue)
                        {
                            litUnitStatusAsOfDate.Text = unit.UnitStatusAsOfDate.Value.ConvertForLiteral();
                            UnitAsOfDt.Text = unit.UnitStatusAsOfDate.HasValue ? unit.UnitStatusAsOfDate.Value.ConvertForDatePicker() : "";
                        }
                    }

                    // inputs

                    NewUnit.SelectedValue = unit.ClientPortalUnitStatusCode;
                    ExemptReason.Text = unit.ExemptionReason;

                    OccupiedBy.Text = unit.OccupiedBy;
                    ContractNo.Text = unit.ContractNo;
                    CommUseDesc.Text = unit.CommUseDesc;
                    CommResYN.SelectedValue = unit.CommResYN;
                    CommZoneUse.Text = unit.CommZoneUse;
                    PropMgrName.Text = unit.PropMgrName;
                    PMEmailPhone.Text = unit.PMEmailPhone;
                    PrincResYN.SelectedValue = unit.PrincResYN;
                    MultiUnitYN.SelectedValue = unit.MultiUnitYN;
                    OtherUnits.Text = unit.OtherUnits;
                    TenantNames.Text = unit.TenantNames;
                    TenantContacts.Text = unit.TenantContacts;
                    DeclareInits.Text = unit.DeclarationInitials;
                }
                else
                {
                    Master.ShowErrorModal("Error retrieving Unit details", "UpdateUnit");
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var user = Master.User;
            var unit = user.CurrentUnit;
            var provider = Master.DataProvider;

            try
            {
                hdnPostback.Value = "true";

                unit.ClientPortalUnitStatusCode = NewUnit.SelectedValue; // Rented or Exempt
                unit.OtherExemptionReason = OtherList.SelectedValue ?? "";
                unit.OccupiedBy = litUnitOccBy.Text;
                unit.ContractNo = ContractNo.Text;
                unit.CommUseDesc = CommUseDesc.Text;
                unit.CommResYN = CommResYN.SelectedValue ?? "";
                unit.CommZoneUse = CommZoneUse.Text;
                unit.PropMgrName = PropMgrName.Text;
                unit.PMEmailPhone = PMEmailPhone.Text;
                unit.PrincResYN = PrincResYN.SelectedValue ?? "";
                unit.MultiUnitYN = MultiUnitYN.SelectedValue ?? "";
                unit.OtherUnits = OtherUnits.Text;
                unit.TenantNames = TenantNames.Text;
                unit.TenantContacts = TenantContacts.Text;
                unit.DeclarationInitials = DeclareInits.Text;

                if (unit.IsRented)
                {
                    unit.TenancyStartDate = provider.GetOptionalDate(UnitAsOfDt.Text);
                }
                else
                {
                    unit.UnitStatusAsOfDate = provider.GetOptionalDate(StartDt.Text);

                    if (unit.ExemptionReason.ToUpper() == "OTHER")
                    {
                        unit.ExemptionReason = unit.OtherExemptionReason;
                    }
                    else
                    {
                        unit.ExemptionReason = ExemptReason.Text;
                    }
                }

                user.CurrentUnit = unit;

                if (provider.UpdateUnit(ref user))
                {
                    if (user.Cart.Items.FindAll(x => x.PropertyId == user.CurrentProperty.PropertyID).Count > 0)
                    {
                        user.Cart.Items.RemoveAll(x => x.PropertyId == user.CurrentProperty.PropertyID);
                        user.Cart.WasUpdated = true;
                        Session["ShowAfterRedirect"] = "Fees may have changed as a result of this update, so please review and add this property to your cart again.|Unit Status Updated";
                    }
                    else
                    {
                        Session["ShowAfterRedirect"] = "Fees may have changed as a result of this update.|Unit Status Updated";
                    }

                    Master.UpdateSession(user);

                    Response.Redirect("~/MyProperties/MyUnits", false);
                }
                else
                {
                    btnSubmit.Attributes.Remove("disabled");
                    UpdateUnitForm.Attributes.Remove("class");

                    Master.ShowErrorModal(provider.ErrorMessage, "Update Unit Status");
                }
            }
            catch (Exception ex) {
                Logger.LogException("UpdateUnit", ex);
                Master.ShowErrorModal("Error updating Unit Status.");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyUnits.aspx", true);
        }
    }
}