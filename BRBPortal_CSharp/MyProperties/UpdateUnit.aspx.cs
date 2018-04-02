using System;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class UpdateUnit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (IsPostBack)
            {
                ConfigureUIAfterPostback();
            }
            else
            {
                var user = Master.User;
                var unit = user.CurrentUnit;
                var provider = Master.DataProvider;

                btnSubmit.Attributes.Add("disabled", "disabled");
                UpdateUnitForm.Attributes.Add("class", "hidden");

                if (provider.GetPropertyUnits(ref user, user.CurrentUnit.UnitID))
                {
                    // literals
                    MainAddress.Text = unit.StreetAddress;
                    UnitNo.Text = unit.UnitNo;
                    UnitStatus.Text = unit.ClientPortalUnitStatusCode;
                    ExemptReas.Text = unit.ExemptionReason;

                    if (string.IsNullOrEmpty(unit.ExemptionReason))
                    {
                        switch (unit.UnitStatCode)
                        {
                            case "NAR":
                                ExemptReas.Text = "Vacant and not available for rent";
                                break;
                            case "OOCC":
                                ExemptReas.Text = "Owner-Occupied";
                                break;
                            case "SEC8":
                                ExemptReas.Text = "Section 8";
                                break;
                            case "FREE":
                                ExemptReas.Text = "Occupied Rent Free";
                                break;
                            case "COMM":
                                ExemptReas.Text = "Commercial Use";
                                break;
                            case "MISC":
                                ExemptReas.Text = "Property Manager's Unit";
                                break;
                            case "SHARED":
                                ExemptReas.Text = "Owner shares kitchen & bath with tenant";
                                break;
                            case "SPLUS":
                                ExemptReas.Text = "Shelter Plus Care";
                                break;
                        }
                    }

                    if (unit.ClientPortalUnitStatusCode == "Rented")
                    {
                        CurrentExemption.Visible = false;
                        if (unit.UnitAsOfDt.HasValue)
                        {
                            UnitStartDt.Text = unit.UnitAsOfDt.Value.ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        CurrentRental.Visible = false;
                    }

                    if (unit.StartDt.HasValue)
                    {
                        StartDt.Text = unit.StartDt.Value.ToString("yyyy-MM-dd");
                    }

                    UnitOccBy.Text = unit.OccupiedBy;

                    // inputs
                    //NewUnit.SelectedValue = unit.ClientPortalUnitStatusCode; // don't pre-select
                    OtherList.SelectedValue = unit.ClientPortalUnitStatusCode; // TODO: see if we get Other back

                    // inputs

                    StartDt.Text = unit.StartDt.HasValue ? unit.StartDt.Value.ToString("yyyy-MM-dd") : "";
                    NewUnit.SelectedValue = unit.ClientPortalUnitStatusCode;
                    ExemptReason.Text = unit.ExemptionReason;
                    UnitAsOfDt.Text = unit.UnitAsOfDt.HasValue ? unit.UnitAsOfDt.Value.ToString("yyyy-MM-dd") : "";
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

        private void ConfigureUIAfterPostback()
        {
            var ExemptGroup_Visible = false;
            var CommUseGrp_Visible = false;
            var PMUnitGrp_Visible = false;
            var OwnerShrGrp_Visible = false;
            var DtStrtdGrp_Visible = false;
            var OccByGrp_Visible = false;
            var ContractGrp_Visible = false;
            var OtherList_Visible = false;

            if (NewUnit.Text == "Rented")
            {
                ExemptGroup_Visible = false;
                CommUseGrp_Visible = false;
                PMUnitGrp_Visible = false;
                OwnerShrGrp_Visible = false;
                DtStrtdGrp_Visible = false;
                OccByGrp_Visible = false;
                ContractGrp_Visible = false;
            }
            else
            {
                ExemptGroup_Visible = true;
                CommUseGrp_Visible = false;
                PMUnitGrp_Visible = false;
                OwnerShrGrp_Visible = false;
                DtStrtdGrp_Visible = false;
                OccByGrp_Visible = false;
                ContractGrp_Visible = false;

                switch (ExemptReason.SelectedValue.ToString().ToUpper())
                {
                    case "NAR": // Vacant and not available for rent
                        ExemptGroup_Visible = true;
                        CommUseGrp_Visible = false;
                        PMUnitGrp_Visible = false;
                        OwnerShrGrp_Visible = false;
                        DtStrtdGrp_Visible = false;
                        OccByGrp_Visible = false;
                        ContractGrp_Visible = false;
                        OtherList_Visible = false;
                        OtherList.Text = "";
                        OtherList.SelectedIndex = -1;
                        break;
                    case "OOCC": // Owner-Occupied
                        ExemptGroup_Visible = true;
                        CommUseGrp_Visible = false;
                        PMUnitGrp_Visible = false;
                        OwnerShrGrp_Visible = false;
                        DtStrtdGrp_Visible = true;
                        OccByGrp_Visible = true;
                        ContractGrp_Visible = false;
                        OtherList_Visible = false;
                        OtherList.Text = "";
                        OtherList.SelectedIndex = -1;
                        break;
                    case "SEC8": // Section 8
                        ExemptGroup_Visible = true;
                        CommUseGrp_Visible = false;
                        PMUnitGrp_Visible = false;
                        OwnerShrGrp_Visible = false;
                        DtStrtdGrp_Visible = true;
                        OccByGrp_Visible = false;
                        ContractGrp_Visible = true;
                        OtherList_Visible = false;
                        OtherList.Text = "";
                        OtherList.SelectedIndex = -1;
                        break;
                    case "FREE": // Occupied Rent Free
                        ExemptGroup_Visible = true;
                        CommUseGrp_Visible = false;
                        PMUnitGrp_Visible = false;
                        OwnerShrGrp_Visible = false;
                        DtStrtdGrp_Visible = true;
                        OccByGrp_Visible = true;
                        ContractGrp_Visible = false;
                        OtherList_Visible = false;
                        OtherList.Text = "";
                        OtherList.SelectedIndex = -1;
                        break;
                    case "OTHER":
                        ExemptGroup_Visible = true;
                        CommUseGrp_Visible = false;
                        PMUnitGrp_Visible = false;
                        OwnerShrGrp_Visible = false;
                        DtStrtdGrp_Visible = false;
                        OccByGrp_Visible = false;
                        ContractGrp_Visible = false;
                        OtherList_Visible = true;
                        OtherList.Text = "";
                        OtherList.SelectedIndex = -1;
                        break;
                }
            }

            if (!ExemptGroup_Visible)
            {
                ExemptGroup.Attributes.Add("class", "hidden");
            }
            if (!CommUseGrp_Visible)
            {
                CommUseGrp.Attributes.Add("class", "hidden");
            }
            if (!PMUnitGrp_Visible)
            {
                PMUnitGrp.Attributes.Add("class", "hidden");
            }
            if (!OwnerShrGrp_Visible)
            {
                OwnerShrGrp.Attributes.Add("class", "hidden");
            }
            if (!DtStrtdGrp_Visible)
            {
                DtStrtdGrp.Attributes.Add("class", "hidden");
            }
            if (!OccByGrp_Visible)
            {
                OccByGrp.Attributes.Add("class", "hidden");
            }
            if (!ContractGrp_Visible)
            {
                ContractGrp.Attributes.Add("class", "hidden");
            }
            if (!OtherList_Visible)
            {
                OtherListContainer.Attributes.Add("class", "hidden");
            }

            if (!string.IsNullOrEmpty(UnitAsOfDt.Text))
            {
                AsOfDtGrp.Attributes.Add("class", "hidden");
            }

            InitalEditButtons.Attributes.Add("class", "hidden");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var user = Master.User;
            var unit = user.CurrentUnit;
            var provider = Master.DataProvider;

            try
            {
                InitalEditButtons.Style.Add("display", "none");
                EditUnitStatusPanel.Style.Remove("display");

                unit.ClientPortalUnitStatusCode = NewUnit.SelectedValue; // Rented or Exempt
                unit.OtherExemptionReason = OtherList.SelectedValue ?? "";
                unit.OccupiedBy = UnitOccBy.Text;
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
                unit.StartDt = provider.GetOptionalDate(StartDt.Text);
                unit.UnitAsOfDt = provider.GetOptionalDate(UnitAsOfDt.Text);

                if (unit.ClientPortalUnitStatusCode.ToUpper() == "EXEMPT")
                {
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