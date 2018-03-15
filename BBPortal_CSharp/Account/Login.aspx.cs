using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;

using BRBPortal_CSharp;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register";
            //// Enable this once you have account confirmation enabled for password reset functionality
            ////ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}
            FailureText.Text = "testing";
            FailureText.Visible = true;
        }

        protected void LogIn(object sender, EventArgs e)
        {
            string wstr;
            string wstr2;

            if (IsValid)
            {
                var result = SignInStatus.Success;

                var Pwd = Password.Text;
                var IDCode = UserIDCode.Text ?? string.Empty;
                var tBillCode = BillCode.Text ?? string.Empty;

                result = BRBFunctions_CSharp.UserAuth(IDCode, tBillCode, Pwd);

                if (result == SignInStatus.Success)
                {
                    var ProfileStr = BRBFunctions_CSharp.GetProfile(IDCode, tBillCode);

                    if ((ProfileStr.Length > 0))
                    {
                        wstr2 = ProfileStr;
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        // User Code
                        if ((wstr.Length > wstr.IndexOf("=")))
                        {
                            IDCode = wstr.Substring((wstr.IndexOf("=") + 1));
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        // Billing Code
                        if ((wstr.Length > wstr.IndexOf("=")))
                        {
                            tBillCode = wstr.Substring((wstr.IndexOf("=") + 1));
                        }

                        Session["UserCode"] = IDCode;
                        Session["BillingCode"] = tBillCode;
                        Session["FirstTimeLogin"] = BRBFunctions_CSharp.iFirstlogin;
                        Session["Relationship"] = BRBFunctions_CSharp.iRelate;
                        Session["TempPwd"] = BRBFunctions_CSharp.iTempPwd;

                        if ((BRBFunctions_CSharp.iTempPwd.ToUpper() == "TRUE"))
                        {
                            wstr = "";
                            if ((BRBFunctions_CSharp.iFirstlogin.ToUpper() == "TRUE")) // comparing with True in VB
                            {
                                Session["NextPage"] = "ProfileConfirm";
                            }
                            else
                            {
                                Session["NextPage"] = "Home";
                            }

                            Response.Redirect("~/Account/ManagePassword.aspx");
                        }
                        else if ((BRBFunctions_CSharp.iFirstlogin.ToUpper() == "TRUE"))
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
            }
        }

        //protected void LogIn_ORIGINAL(object sender, EventArgs e)
        //{
        //    if (IsValid)
        //    {
        //        // Validate the user password
        //        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

        //        // This doen't count login failures towards account lockout
        //        // To enable password failures to trigger lockout, change to shouldLockout: true
        //        var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

        //        switch (result)
        //        {
        //            case SignInStatus.Success:
        //                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
        //                break;
        //            case SignInStatus.LockedOut:
        //                Response.Redirect("/Account/Lockout");
        //                break;
        //            case SignInStatus.RequiresVerification:
        //                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
        //                                                Request.QueryString["ReturnUrl"],
        //                                                RememberMe.Checked),
        //                                  true);
        //                break;
        //            case SignInStatus.Failure:
        //            default:
        //                FailureText.Text = "Invalid login attempt";
        //                ErrorMessage.Visible = true;
        //                break;
        //        }
        //    }
        //}
    }
}