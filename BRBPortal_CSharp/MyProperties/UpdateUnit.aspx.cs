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
                var userCode = Session["UserCode"] as String ?? "";
                var billingCode = Session["BillingCode"] as String ?? "";
                var iPropertyNo = Session["PropertyID"] as String ?? "";
                var iPropertyAddress = Session["PropAddr"] as String ?? "";
                iUnitID = Session["UnitID"] as String ?? "";
                iUnitNum = Session["UnitNo"] as String ?? "";
                var propertyBalance = Session["PropBalance"] as String ?? "";

                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
                {
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    var propertyUnits = BRBFunctions_CSharp.GetPropertyUnits(iPropertyNo, userCode, billingCode, iUnitID);

                    if (propertyUnits.Count == 0)
                    {
                        ShowDialogOK("Error: Error retrieving Unit ID " + iUnitID + " Unit No " + iUnitNum + ".", "Update Units");
                        return;
                    }

                    UnitStatus.Text = propertyUnits.GetStringValue("CPStatus");
                    ExemptReas.Text = propertyUnits.GetStringValue("ExReason");
                    UnitStartDt.Text = propertyUnits.GetStringValue("StartDt");
                    UnitOccBy.Text = propertyUnits.GetStringValue("OccBy");
                    hfUnitID.Value = propertyUnits.GetStringValue("UnitID");

                    if (UnitStatus.Text == "Rented")
                    {
                        ExemptReas.Text = "";
                        ExemptReason.SelectedIndex = -1;
                        ExemptGroup.Visible = false;
                        CommUseGrp.Visible = false;
                        PMUnitGrp.Visible = false;
                        OwnerShrGrp.Visible = false;
                        AsOfDtGrp.Visible = false;
                        DtStrtdGrp.Visible = false;
                        OccByGrp.Visible = false;
                        ContractGrp.Visible = false;
                    }
                    else
                    {
                        ExemptReason.SelectedValue = ExemptReas.Text;
                        ExemptGroup.Visible = true;
                        CommUseGrp.Visible = false;
                        PMUnitGrp.Visible = false;
                        OwnerShrGrp.Visible = false;
                        AsOfDtGrp.Visible = false;
                        DtStrtdGrp.Visible = false;
                        OccByGrp.Visible = false;
                        ContractGrp.Visible = false;

                        switch (ExemptReason.SelectedValue.ToString().ToUpper())
                        {
                            case "NAR":
                                ExemptGroup.Visible = true;
                                CommUseGrp.Visible = false;
                                PMUnitGrp.Visible = false;
                                OwnerShrGrp.Visible = false;
                                AsOfDtGrp.Visible = true;
                                DtStrtdGrp.Visible = false;
                                OccByGrp.Visible = false;
                                ContractGrp.Visible = false;
                                OtherList.Visible = false;
                                OtherList.ClearSelection();
                               break;
                            case "OOCC":
                                ExemptGroup.Visible = true;
                                CommUseGrp.Visible = false;
                                PMUnitGrp.Visible = false;
                                OwnerShrGrp.Visible = false;
                                AsOfDtGrp.Visible = false;
                                DtStrtdGrp.Visible = true;
                                OccByGrp.Visible = true;
                                ContractGrp.Visible = false;
                                OtherList.Visible = false;
                                OtherList.ClearSelection();
                                break;
                            case "SEC8":
                                ExemptGroup.Visible = true;
                                CommUseGrp.Visible = false;
                                PMUnitGrp.Visible = false;
                                OwnerShrGrp.Visible = false;
                                AsOfDtGrp.Visible = false;
                                DtStrtdGrp.Visible = true;
                                OccByGrp.Visible = false;
                                ContractGrp.Visible = true;
                                OtherList.Visible = false;
                                OtherList.ClearSelection();
                                break;
                            case "FREE":
                                ExemptGroup.Visible = true;
                                CommUseGrp.Visible = false;
                                PMUnitGrp.Visible = false;
                                OwnerShrGrp.Visible = false;
                                AsOfDtGrp.Visible = false;
                                DtStrtdGrp.Visible = true;
                                OccByGrp.Visible = true;
                                ContractGrp.Visible = false;
                                OtherList.Visible = false;
                                OtherList.ClearSelection();
                                break;
                            case "OTHER":
                                ExemptGroup.Visible = true;
                                CommUseGrp.Visible = false;
                                PMUnitGrp.Visible = false;
                                OwnerShrGrp.Visible = false;
                                AsOfDtGrp.Visible = false;
                                DtStrtdGrp.Visible = false;
                                OccByGrp.Visible = false;
                                ContractGrp.Visible = false;
                                OtherList.Visible = true;
                                OtherList.ClearSelection();
                                break;
                        }
                    }
                }

                MainAddress.Text = iPropertyAddress;
                UnitNo.Text = iUnitNum;
                NewUnit.SelectedValue = UnitStatus.Text;
                FailureText.Text = "";
                ErrorMessage.Visible = true;
            }
        }

        protected void UpdateUnit_Click(object sender, EventArgs e)
        {
        }

        protected void CancelEdit_Click(object sender, EventArgs e)
        {
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}