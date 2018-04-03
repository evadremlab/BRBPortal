using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace BRBPortal_CSharp.Account
{
    public partial class ManagePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Master.User;

            if (Session["NextPage"] == null)
            {
                Session["NextPage"] = "ProfileList";
            }

            hdnPassword.Value = user.Password;
        }

        protected void ChangePassword()
        {
            var user = Master.User;
            var provider = Master.DataProvider;

            if (provider.Authenticate(ref user, user.Password) != SignInStatus.Success)
            {
                Master.ShowErrorModal("Current password is incorrect.", "Change Password");
                return;
            }

            if (!provider.UpdateUserPassword(user, user.Password, NewPWD.Text, ConfirmNewPassword.Text))
            {
                var errorMessage = provider.ErrorMessage;

                if (errorMessage.IndexOf("password policy") != -1)
                {
                    errorMessage = "Your new password must contain at least 8 characters, have both upper and lower case letters, and not contain your user id.";
                }

                Master.ShowErrorModal("Error changing password: " + errorMessage, "Change Password");
                return;
            }

            Master.UpdateSession(user);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.BillingCode));
            Request.GetOwinContext().Authentication.SignIn(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));

            Session["ShowAfterRedirect"] = "Your password has been reset.|Password Reset";

            if (Session["NextPage"].ToString() == "ProfileConfirm")
            {
                Response.Redirect("~/Account/ProfileConfirm", false);
            }
            else if (Session["NextPage"].ToString() == "ProfileList")
            {
                Response.Redirect("~/Account/ProfileList", false);
            }
            else
            {
                Response.Redirect("~/Home", false);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChangePassword();
        }
    }
}