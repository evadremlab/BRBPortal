using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BRBPortal_CSharp.Account
{
    public partial class ManagePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["NextPage"] == null)
            {
                Session["NextPage"] = "ProfileList";
            }

            if (IsPostBack)
            {
                ChangePassword();
            }
        }

        protected void ChangePassword()
        {
            var result = SignInStatus.Success;

            var userCode = Session["UserCode"] as String ?? "";
            var billingCode = Session["BillingCode"] as String ?? "";

            ShowDialogOK("Password rules Not met. Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id.", "Change Password");
            return;

            result = BRBFunctions_CSharp.UserAuth(userCode, billingCode, CurrentPassword.Text);

            if (result == SignInStatus.Failure)
            {
                ShowDialogOK("Current password is incorrect.", "Change Password");
                return;
            }

            if (BRBFunctions_CSharp.CheckPswdRules(NewPWD.Text, userCode) == false)
            {
                ShowDialogOK("Password rules Not met. Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id.", "Change Password");
                return;
            }

            if (BRBFunctions_CSharp.UpdatePassword(userCode, billingCode, CurrentPassword.Text.EscapeXMLChars(), NewPWD.Text.EscapeXMLChars(), ConfirmNewPassword.Text.EscapeXMLChars()) == false)
            {
                ShowDialogOK("Error changing password: " + BRBFunctions_CSharp.iErrMsg, "Change Password");
                return;
            }

            if (Session["NextPage"].ToString() == "ProfileConfirm")
            {
                Response.Redirect("~/Account/ProfileConfirm.aspx", false);
            }
            else if (Session["NextPage"].ToString() == "ProfileList")
            {
                Response.Redirect("~/Account/ProfileList.aspx", false);
            }
            else
            {
                Response.Redirect("~/Home.aspx", false);
            }
        }

        protected void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}