using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.Account
{
    public partial class ProfileList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userCode = Session["UserCode"] as String;
                string billingCode = Session["BillingCode"] as String;
                string relationship = Session["Relationship"] as String;

                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
                {
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    UserIDCode1.Text = "";
                    BillCode1.Text = "";
                    FullName1.Text = "";
                    MailAddress1.Text = "";
                    EmailAddress1.Text = "";
                    PhoneNo1.Text = "";
                    Quest1.Text = "";
                    Quest2.Text = "";

                    var RetStr = BRBFunctions_CSharp.GetProfile(userCode, billingCode);

                    if (RetStr.Length > 0)
                    {
                        var wstr2 = RetStr;
                        var wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            UserIDCode1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            BillCode1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            FullName1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            MailAddress1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            EmailAddress1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            PhoneNo1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            Quest1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("="))
                        {
                            Quest2.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }

                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");
                        wstr = BRBFunctions_CSharp.ParseStr(ref wstr2, "::");

                        if (wstr.Length > wstr.IndexOf("=") && relationship.ToUpper() == "AGENT")
                        {
                            FullName1.Text = wstr.Substring(wstr.IndexOf("=") + 1).Trim();
                        }
                    }
                }
            }
        }

        protected void EditProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ProfileEdit.aspx", false);
        }

        protected void CancelList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx", false);
        }
    }
}