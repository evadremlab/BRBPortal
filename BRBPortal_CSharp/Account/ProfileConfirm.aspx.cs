using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.Account
{
    public partial class ProfileConfirm : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack == true)
            {
                string userCode = Session["UserCode"] as String;
                string billingCode = Session["BillingCode"] as String;
                string relationship = Session["Relationship"] as String;

                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
                {
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    UserIDCode0.Text = "";
                    BillCode0.Text = "";
                    FullName0.Text = "";
                    MailAddress0.Text = "";
                    EmailAddress0.Text = "";
                    PhoneNo0.Text = "";

                    var RetStr = BRBFunctions_CSharp.GetProfile(userCode, billingCode);

                    if (RetStr.Length > 0)
                    {
                        var wstr2 = RetStr;
                        var wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            UserIDCode0.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            BillCode0.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            FullName0.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            MailAddress0.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            EmailAddress0.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            PhoneNo0.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if ((wstr.Length > wstr.IndexOf("=")) && relationship.ToUpper() == "AGENT")
                        {
                            FullName0.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }
                    }
                }

                btnSubmit.Enabled = false;
            }
        }

        protected void SubmitProfile_Click(object sender, EventArgs e)
        {
            var success = BRBFunctions_CSharp.ConfirmProfile(UserIDCode0.Text, BillCode0.Text, DeclareInits.Text);

            if (!success)
            {
                ShowDialogOK("Error updating confirmation.", "Confirm Profile");
            }

            Response.Redirect("~/Home", false);
        }

        protected void CancelProfile_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Account/Login", false);
        }

        protected void chkDeclare_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeclare.Checked == true && DeclareInits.Text.Length > 0)
            {
                btnSubmit.Enabled = true;
            }
            else
            {
                btnSubmit.Enabled = false;
            }
        }

        protected void DeclareInits_TextChanged(object sender, EventArgs e)
        {
            if (DeclareInits.Text.Length > 0 && chkDeclare.Checked == true)
            {
                btnSubmit.Enabled = true;
            }
            else
            {
                btnSubmit.Enabled = false;
            }
        }

        protected void ShowDialogOK(string aMessage, string aTitle = "Status")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopupOK('" + aMessage + "', '" + aTitle + "');", true);
        }

        protected void ShowDialogYN(string aDialogID, string aMessage, string aTitle, string aDialogData = "")
        {
            hfDialogID.Value = aDialogID;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopupYN('" + aMessage + "', '" + aTitle + "');", true);
        }

        protected void DialogResponseYes(Object sender, EventArgs e)
        {
            // cannot auto-convert VB.NET handler
        }

        protected void DialogResponseNo(Object sender, EventArgs e)
        {
            // cannot auto-convert VB.NET handler
        }
    }
}