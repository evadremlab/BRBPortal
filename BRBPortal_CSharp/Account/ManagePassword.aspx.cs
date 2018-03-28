using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;

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
            var user = Master.User;

            if (BRBFunctions_CSharp.UserAuth(ref user, CurrentPassword.Text) != SignInStatus.Success)
            {
                Master.ShowDialogOK("Current password is incorrect.", "Change Password");
                return;
            }

            if (!BRBFunctions_CSharp.CheckPswdRules(user, NewPWD.Text))
            {
                Master.ShowDialogOK("Password rules Not met. Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id.", "Change Password");
                return;
            }

            if (!BRBFunctions_CSharp.UpdatePassword(user, CurrentPassword.Text.EscapeXMLChars(), NewPWD.Text.EscapeXMLChars(), ConfirmNewPassword.Text.EscapeXMLChars()))
            {
                Master.ShowDialogOK("Error changing password: " + BRBFunctions_CSharp.iErrMsg, "Change Password");
                return;
            }

            Master.UpdateSession(user);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.BillingCode));
            Request.GetOwinContext().Authentication.SignIn(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));

            if (Session["NextPage"].ToString() == "ProfileConfirm")
            {
                Response.Redirect("~/Account/ProfileConfirm");
            }
            else if (Session["NextPage"].ToString() == "ProfileList")
            {
                Response.Redirect("~/Account/ProfileList");
            }
            else
            {
                Response.Redirect("~/Home");
            }
        }
    }
}