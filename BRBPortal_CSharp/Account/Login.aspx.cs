using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.UI;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.Account
{
    /// <summary>
    /// DONE
    /// </summary>
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Session.Clear();

                var user = new BRBUser();

                if (UserIDOrBillCode.SelectedValue == "UserID")
                {
                    user.UserCode = UserIDCode.Text ?? "";
                }
                else
                {
                    user.BillingCode = BillCode.Text ?? "";
                }

                if (BRBFunctions_CSharp.UserAuth(ref user, Password.Text ?? "") == SignInStatus.Success)
                {
                    Master.UpdateSession(user);

                    if (user.IsTemporaryPassword)
                    {
                        Session["NextPage"] = user.IsFirstlogin ? "ProfileConfirm" : "Home";
                        Response.Redirect("~/Account/ManagePassword.aspx");
                    }
                    else if (user.IsFirstlogin)
                    {
                        Session["NextPage"] = "Home";
                        Response.Redirect("~/Account/ProfileConfirm.aspx");
                    }
                    else
                    {
                        if (BRBFunctions_CSharp.GetProfile(ref user))
                        {
                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, user.BillingCode));
                            Request.GetOwinContext().Authentication.SignIn(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));

                            Master.UpdateSession(user);

                            Response.Redirect("~/Home.aspx", true);
                        }
                        else
                        {
                            FailureText.Text = "Invalid login attempt";
                            ErrorMessage.Visible = true;
                            Session.Clear();
                        }
                    }
                }
                else
                {
                    FailureText.Text = "Invalid login attempt";
                    ErrorMessage.Visible = true;
                    Session.Clear();
                }
            }
        }
    }
}