using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class UpdateUnit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (IsPostBack)
            {
                InitalEditButtons.Style.Add("display", "none");
                EditUnitStatusPanel.Style.Remove("display");
                btnConfirm.Enabled = true;
            }
            else
            {
                var user = Master.User;
                var unit = user.CurrentUnit;

                if (BRBFunctions_CSharp.GetPropertyUnits(ref user, user.CurrentUnit.UnitID))
                {
                    // literals
                    MainAddress.Text = unit.StreetAddress;
                    UnitNo.Text = unit.UnitNo;
                    UnitStatus.Text = unit.ClientPortalUnitStatusCode;
                    ExemptReas.Text = unit.ExemptionReason;

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
                    Master.ShowDialogOK("Error retrieving Unit details", "UpdateUnit");
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var user = Master.User;
            var unit = user.CurrentUnit;

            // literals
            MainAddress.Text = unit.StreetAddress;
            UnitNo.Text = unit.UnitNo;
            UnitStatus.Text = unit.ClientPortalUnitStatusCode;
            ExemptReas.Text = unit.ExemptionReason;

            // inputs

            if (!string.IsNullOrEmpty(StartDt.Text))
            {
                DateTime dtStartDate = DateTime.MinValue;

                if (DateTime.TryParse(StartDt.Text, out dtStartDate))
                {
                    unit.StartDt = dtStartDate;
                }
            }

            unit.OccupiedBy = UnitOccBy.Text;
            unit.ClientPortalUnitStatusCode = NewUnit.SelectedValue; // Rented or Exempt

            unit.ExemptionReason = ExemptReason.Text;
            unit.OtherExemptionReason = OtherList.SelectedValue ?? "";

            if (string.IsNullOrEmpty(unit.ExemptionReason) || !Regex.IsMatch(unit.ExemptionReason, "OOCC|FREE"))
            {
                unit.OccupiedBy = "";
            }

            if (!string.IsNullOrEmpty(UnitAsOfDt.Text))
            {
                var dtChangeDate = DateTime.MinValue;

                if (DateTime.TryParse(UnitAsOfDt.Text, out dtChangeDate))
                {
                    unit.UnitAsOfDt = dtChangeDate;
                }
            }

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

            user.CurrentUnit = unit;

            if (BRBFunctions_CSharp.SaveUnit(ref user))
            {
                Master.ShowDialogOK("Updated Unit Status.", "Update Unit Status");
            }
            else
            {
                if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                {
                    BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                }

                Master.ShowDialogOK("Error updating Unit Status: " + BRBFunctions_CSharp.iErrMsg, "Update Unit Status");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyUnits.aspx", true);
        }
    }
}