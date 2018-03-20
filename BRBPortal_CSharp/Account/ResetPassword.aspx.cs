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
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            validateReset();
        }

        //protected void SecurityAnswer_TextChanged(object sender, EventArgs e)
        //{
        //    if ((Answer1.Text.Length > 0 && Quest1.Text.Length > 0) || (Answer2.Text.Length > 0 && Quest2.Text.Length > 0))
        //    {
        //        btnResetPWD.Enabled = true;
        //    }
        //    else
        //    {
        //        btnResetPWD.Enabled = false;
        //    }
        //}

        protected void UserIDCode_TextChanged(object sender, EventArgs e)
        {
            validateReset();
        }

        protected void BillingCode_TextChanged(object sender, EventArgs e)
        {
            validateReset();
        }

        private void ShowDialog(string message, string title)
        {
            var fn = string.Format("showOkModalOnPostback('{0}', '{1}';", message, title);

            ClientScript.RegisterStartupScript(this.GetType(), "Javascript", fn, true);
        }

        private void validateReset()
        {
            var userProfile = new UserProfile
            {
                UserCode = UserIDCode.Text,
                BillingCode = BillingCode.Text,
                Question1 = Quest1.Text,
                Question2 = Quest2.Text,
                Answer1 = Answer1.Text,
                Answer2 = Answer2.Text
            };

            if (BRBFunctions_CSharp.ValidateReset(userProfile))
            {
                ShowDialog("Temporary password has been sent. Please login using temporary password.", "Forgot Password");

                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                ShowDialog("Security answer(s) did not match.", "Reset Password");
            }
        }
    }
}