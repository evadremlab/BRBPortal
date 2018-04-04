using System;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class UpdateUnit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = Master.User;
                var unit = user.CurrentUnit;
                var provider = Master.DataProvider;

                UpdateUnitForm.Style["display"] = "none";
                btnSubmit.Attributes.Add("disabled", "disabled");

                if (provider.GetPropertyUnits(ref user, user.CurrentUnit.UnitID))
                {
                    unit = user.CurrentUnit;

                    litMainAddress.Text = unit.StreetAddress;
                    litUnitNo.Text = unit.UnitNo;
                    litUnitStatus.Text = unit.ClientPortalUnitStatusCode;
                    litExemptReas.Text = unit.ExemptionReason;
                    // so we can access them client-side
                    hdnUnitStatus.Value = unit.ClientPortalUnitStatusCode;
                    hdnExemptReas.Value = unit.UnitStatCode;

                    Master.UpdateSession(user);

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
                        litUnitOccBy.Text = unit.OccupiedBy;
                    }
                    else
                    {
                        CurrentRental.Visible = false;
                    }

                    if (unit.UnitStatusAsOfDate.HasValue)
                    {
                        litUnitStatusAsOfDate.Text = unit.UnitStatusAsOfDate.Value.ConvertForLiteral();
                    }
                }
                else
                {
                    Master.ShowErrorModal("Error retrieving Unit details", "UpdateUnit");
                }
            }
        }

        // Unit status change date cannot be same or before current status date. Please select appropriate date and try again.

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var user = Master.User;
            var unit = user.CurrentUnit;
            var provider = Master.DataProvider;

            try
            {
                hdnPostback.Value = "true";

                unit.ClientPortalUnitStatusCode = NewUnit.SelectedValue; // Rented or Exempt
                unit.ExemptionReason = ExemptReason.SelectedValue ?? "";
                unit.OtherExemptionReason = OtherList.SelectedValue ?? "";
                unit.OccupiedBy = OccupiedBy.Text;
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

                if (!string.IsNullOrEmpty(UnitAsOfDt.Text))
                {
                    unit.StartDate = null;
                    unit.AsOfDate = provider.GetOptionalDate(UnitAsOfDt.Text);
                }
                else if (!string.IsNullOrEmpty(StartDt.Text))
                {
                    unit.AsOfDate = null;
                    unit.StartDate = provider.GetOptionalDate(StartDt.Text);
                }

                if (unit.IsExempt)
                {
                    if (unit.ExemptionReason.ToUpper() == "OTHER")
                    {
                        unit.ExemptionReason = unit.OtherExemptionReason;
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