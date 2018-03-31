using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.UI;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;
using System.Xml.Linq;
using System.Xml;

namespace BRBPortal_CSharp.Account
{
    /// <summary>
    /// DONE
    /// </summary>
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var showTempPasswordMsg = Session["ShowTemporaryPasswordMsg"] as String ?? "";

            if (showTempPasswordMsg == "TRUE")
            {
                TemporaryPasswordMsg.Visible = true;
                Session.Remove("ShowTemporaryPasswordMsg");
            }
            else
            {
                TemporaryPasswordMsg.Visible = false;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            Session.Clear();

            var user = new BRBUser();

            if (IsValid)
            {
                try
                {
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
                        if (BRBFunctions_CSharp.GetProfile(ref user))
                        {
                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, user.BillingCode));
                            Request.GetOwinContext().Authentication.SignIn(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));

                            Master.UpdateSession(user);

                            if (user.IsTemporaryPassword)
                            {
                                Session["NextPage"] = user.IsFirstlogin ? "ProfileConfirm" : "Home";
                                Response.Redirect("~/Account/ManagePassword", false);
                            }
                            else if (user.IsFirstlogin)
                            {
                                Session["NextPage"] = "Home";
                                Response.Redirect("~/Account/ProfileConfirm", false);
                            }
                            else
                            {
                                Response.Redirect("~/Home", false);
                            }
                        }
                        else
                        {
                            Session.Clear();
                            Logger.Log("Login", BRBFunctions_CSharp.iErrMsg);
                            Master.ShowErrorModal("Error getting User Profile.", "Login Error");
                        }
                    }
                    else
                    {
                        Session.Clear();
                        Master.ShowErrorModal("Authentication Error.", "Login Error");
                    }
                }
                catch (Exception ex)
                {
                    Session.Clear();
                    Logger.LogException("Login", ex);
                    Master.ShowErrorModal("Please contact your system administrator.", "System Error");
                }
            }
        }
    }
}