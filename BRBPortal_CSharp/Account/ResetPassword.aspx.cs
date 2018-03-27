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
                ErrMessage.Visible = false;
                btnResetPWD.Enabled = false;
                UserIDCode.Focus();
            }
        }

        protected void UserIDCode_Or_BillingCode_TextChanged(object sender, EventArgs e)
        {
            var user = new BRBUser
            {
                UserCode = UserIDCode.Text,
                BillingCode = BillingCode.Text
            };

            if (BRBFunctions_CSharp.GetProfile(ref user))
            {
                Master.UpdateSession(user);

                Quest1.Text = Master.User.Question1;
                Quest2.Text = Master.User.Question2;
                Answer1.Focus();
            }
            else
            {
                Master.ShowDialogOK("Error: Invalid User ID or Billing Code.", "Reset Password");
                UserIDCode.Focus();
                return;
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            var user = Master.User;

            if (BRBFunctions_CSharp.ValidateReset(ref user))
            {
                Master.UpdateSession(user);
                Master.ShowDialogOK("Temporary password has been sent. Please login using temporary password.", "Forgot Password");

                Response.Redirect("~/Account/Login.aspx", true);
            }
            else
            {
                Master.ShowDialogOK("Security answer(s) did not match.", "Reset Password");
            }
        }
    }
}