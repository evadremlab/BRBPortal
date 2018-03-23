using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.Account
{
    public partial class ResetPassword : Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                Quest1.Text = "";
                Quest2.Text = "";
                FailureText.Text = "";
                ErrMessage.Visible = false;
                btnResetPWD.Enabled = false;
                UserIDCode.Focus();
            }
        }

        protected void UserIDCode_Or_BillingCode_TextChanged(object sender, EventArgs e)
        {
            var fields = BRBFunctions_CSharp.GetProfile(UserIDCode.Text, BillingCode.Text);

            if (fields.Count > 0)
            {
                UserIDCode.Text = fields.GetStringValue("UserCode");
                BillingCode.Text = fields.GetStringValue("BillingCode");
                Quest1.Text = fields.GetStringValue("Question1");
                Quest2.Text = fields.GetStringValue("Question2");
                Answer1.Focus();
            }
            else
            {
                ShowDialogOK("Error: Invalid User ID or Billing Code.", "Reset Password");
                UserIDCode.Focus();
                return;
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            var profile = new UserProfile
            {
                UserCode = UserIDCode.Text,
                BillingCode = BillingCode.Text,
                Question1 = Quest1.Text,
                Question2 = Quest2.Text,
                Answer1 = Answer1.Text,
                Answer2 = Answer2.Text
            };

            if (BRBFunctions_CSharp.ValidateReset(profile))
            {
                ShowDialogOK("Temporary password has been sent. Please login using temporary password.", "Forgot Password");

                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                ShowDialogOK("Security answer(s) did not match.", "Reset Password");
            }
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}