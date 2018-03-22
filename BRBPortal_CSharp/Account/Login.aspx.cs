using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;

using BRBPortal_CSharp;
using BRBPortal_CSharp.Models;
using System.Collections.Generic;
using System.Security.Claims;

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
                var userCode = UserIDCode.Text ?? string.Empty;
                var billCode = BillCode.Text ?? string.Empty;
                var password = Password.Text ?? string.Empty;

                var result = BRBFunctions_CSharp.UserAuth(userCode, billCode, password);

                if (result == SignInStatus.Success)
                {
                    var fields = BRBFunctions_CSharp.GetProfile(userCode, billCode);

                    if (fields.Count > 0)
                    {
                        userCode = fields.GetStringValue("UserCode");
                        billCode = fields.GetStringValue("BillingCode");

                        Session["UserCode"] = userCode;
                        Session["BillingCode"] = billCode;
                        Session["FirstTimeLogin"] = BRBFunctions_CSharp.iFirstlogin;
                        Session["Relationship"] = BRBFunctions_CSharp.iRelate;
                        Session["TempPwd"] = BRBFunctions_CSharp.iTempPwd;

                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, billCode));
                        Request.GetOwinContext().Authentication.SignIn(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));

                        if (BRBFunctions_CSharp.iTempPwd.ToUpper() == "TRUE")
                        {
                            if ((BRBFunctions_CSharp.iFirstlogin.ToUpper() == "TRUE"))
                            {
                                Session["NextPage"] = "ProfileConfirm";
                            }
                            else
                            {
                                Session["NextPage"] = "Home";
                            }

                            Response.Redirect("~/Account/ManagePassword.aspx");
                        }
                        else if (BRBFunctions_CSharp.iFirstlogin.ToUpper() == "TRUE")
                        {
                            Session["NextPage"] = "Home";
                            Response.Redirect("~/Account/ProfileConfirm.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/Home.aspx");
                        }
                    }
                    else
                    {
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        Session.Clear();
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