using System;
using BRBPortal_CSharp.DAL;

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

                    var ExemptGroup_Visible = false;
                    var CommUseGrp_Visible = false;
                    var PMUnitGrp_Visible = false;
                    var OwnerShrGrp_Visible = false;
                    var AsOfDtGrp_Visible = false; // hide until editing
                    var DtStrtdGrp_Visible = false;
                    var OccByGrp_Visible = false;
                    var ContractGrp_Visible = false;
                    var OtherList_Visible = false;

                    if (UnitStatus.Text == "Rented")
                    {
                        ExemptReason.SelectedIndex = -1;
                        ExemptReason.Text = "";
                        ExemptGroup_Visible = false;
                        CommUseGrp_Visible = false;
                        PMUnitGrp_Visible = false;
                        OwnerShrGrp_Visible = false;
                        AsOfDtGrp_Visible = false; // hide until editing
                        DtStrtdGrp_Visible = false;
                        OccByGrp_Visible = false;
                        ContractGrp_Visible = false;
                    }
                    else
                    {
                        ExemptReason.SelectedValue = ExemptReas.Text;
                        ExemptGroup_Visible = true;
                        CommUseGrp_Visible = false;
                        PMUnitGrp_Visible = false;
                        OwnerShrGrp_Visible = false;
                        AsOfDtGrp_Visible = false;
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
                                AsOfDtGrp_Visible = true;
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
                                AsOfDtGrp_Visible = false;
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
                                AsOfDtGrp_Visible = false;
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
                                AsOfDtGrp_Visible = false;
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
                                AsOfDtGrp_Visible = false;
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
                        ExemptGroup.Attributes["class"] = "hidden";
                    }
                    if (!CommUseGrp_Visible)
                    {
                        CommUseGrp.Attributes["class"] = "hidden";
                    }
                    if (!PMUnitGrp_Visible)
                    {
                        PMUnitGrp.Attributes["class"] = "hidden";
                    }
                    if (!OwnerShrGrp_Visible)
                    {
                        OwnerShrGrp.Attributes["class"] = "hidden";
                    }
                    if (!AsOfDtGrp_Visible)
                    {
                        AsOfDtGrp.Attributes["class"] = "hidden";
                    }
                    if (!DtStrtdGrp_Visible)
                    {
                        DtStrtdGrp.Attributes["class"] = "hidden";
                    }
                    if (!OccByGrp_Visible)
                    {
                        OccByGrp.Attributes["class"] = "hidden";
                    }
                    if (!ContractGrp_Visible)
                    {
                        ContractGrp.Attributes["class"] = "hidden";
                    }

                    if (!OtherList_Visible)
                    {
                        OtherListContainer.Attributes["class"] = "hidden";
                    }
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
                Master.UpdateSession(user);
                Master.ShowOKModal("Updated Unit Status.", "Update Unit Status");
            }
            else
            {
                Master.ShowErrorModal(provider.ErrorMessage, "Update Unit Status");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyUnits.aspx", true);
        }
    }
}