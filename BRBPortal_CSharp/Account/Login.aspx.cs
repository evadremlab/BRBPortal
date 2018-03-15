using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using System.Net;
using System.Xml;
using System.IO;
using System;

/// <summary>
/// CONVERTED from VB.NET
/// </summary>
namespace BRBPortal_CSharp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnLogin.UniqueID;
            UserIDCode.Focus();
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
    }
}
