using System;
using System.Web.UI;
using BRBPortal_CSharp.DAL;
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
            var provider = new DataProvider();

            var user = new BRBUser
            {
                UserCode = UserIDCode.Text,
                BillingCode = BillingCode.Text
            };

            if (provider.GetUserProfile(ref user))
            {
                Master.UpdateSession(user);

                if (string.IsNullOrEmpty(Master.User.Question1) || string.IsNullOrEmpty(Master.User.Question2))
                {
                    Master.ShowErrorModal("Your profile does not have security questions assigned. Please contact the system administrator.", "Profile Error", 250);
                }

                Quest1.Text = Master.User.Question1;
                Quest2.Text = Master.User.Question2;
                Answer1.Focus();
            }
            else
            {
                Master.ShowErrorModal("Error: Invalid User ID or Billing Code.", "Reset Password");
                UserIDCode.Focus();
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            var user = Master.User;
            var provider = new DataProvider();

            if (UserIDOrBillCode.SelectedValue == "UserID")
            {
                user.UserCode = UserIDCode.Text ?? "";
            }
            else
            {
                user.BillingCode = BillingCode.Text ?? "";
            }

            if (provider.ResetUserPassword(ref user))
            {
                Master.UpdateSession(user);

                Session["ShowTemporaryPasswordMsg"] = "TRUE";
                Response.Redirect("~/Account/Login", false);
            }
            else
            {
                Master.ShowErrorModal("Security answer(s) did not match.", "Reset Password");
            }
        }
    }
}