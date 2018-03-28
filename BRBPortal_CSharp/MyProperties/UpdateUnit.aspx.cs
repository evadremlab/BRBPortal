using System;
using System.Collections.Generic;
using System.Linq;
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
                BRBFunctions_CSharp.GetPropertyUnits(ref user, user.CurrentUnit.UnitID);

                EditUnitStatusPanel.Style.Add("display", "none");

                PropertyAddress.Text = user.CurrentUnit.StreetAddress;
                UnitStatus.Text = user.CurrentUnit.ClientPortalUnitStatusCode;
                UnitNo.Text = user.CurrentUnit.UnitNo;
                NewUnit.SelectedValue = UnitStatus.Text;
                ExemptReas.Text = user.CurrentUnit.ExemptionReason;
                UnitOccBy.Text = user.CurrentUnit.OccupiedBy;
                hfUnitID.Value = user.CurrentUnit.UnitID;

                if (user.CurrentUnit.StartDt.HasValue)
                {
                    UnitStartDt.Text = user.CurrentUnit.StartDt.Value.ToString("MM/dd/yyyy");
                }

                var ExemptGroup_Visible = false;
                var CommUseGrp_Visible = false;
                var PMUnitGrp_Visible = false;
                var OwnerShrGrp_Visible = false;
                var AsOfDtGrp_Visible = false;
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
                    AsOfDtGrp_Visible = false;
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
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            Master.ShowDialogOK("Update Unit is under construction", "Update Units");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyUnits.aspx", true);
        }
    }
}