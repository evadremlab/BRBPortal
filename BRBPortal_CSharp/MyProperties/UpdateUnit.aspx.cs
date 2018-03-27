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
        private string iPropertyAddress = "";
        private string iUnitNum = "";
        private string iUnitID = "";
        private string iPropertyNo = "";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                var iPropertyNo = Session["PropertyID"] as String ?? "";
                var iPropertyAddress = Session["PropAddr"] as String ?? "";
                iUnitID = Session["UnitID"] as String ?? "";
                iUnitNum = Session["UnitNo"] as String ?? "";
                var propertyBalance = Session["PropBalance"] as String ?? "";

                var user = Master.User;
                BRBFunctions_CSharp.GetPropertyUnits(ref user, iPropertyNo, iUnitID);

                if (user.CurrentProperty.Units.Count == 0)
                {
                    ShowDialogOK("Error: Error retrieving Unit ID " + iUnitID + " Unit No " + iUnitNum + ".", "Update Units");
                    return;
                }

                UnitStatus.Text = user.CurrentProperty.Units[0].ClientPortalUnitStatusCode;
                ExemptReas.Text = user.CurrentProperty.Units[0].ExemptionReason;

                if (user.CurrentProperty.Units[0].StartDt.HasValue)
                {
                    UnitStartDt.Text = user.CurrentProperty.Units[0].StartDt.Value.ToString("MM/dd/yyyy");
                }

                UnitOccBy.Text = user.CurrentProperty.Units[0].OccupiedBy;
                hfUnitID.Value = user.CurrentProperty.Units[0].UnitID;

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

                MainAddress.Text = iPropertyAddress;
                UnitNo.Text = iUnitNum;
                NewUnit.SelectedValue = UnitStatus.Text;
            }
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            ShowDialogOK("Update Unit is under construction", "Update Units");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyUnits.aspx", true);
        }
    }
}