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
                string userCode = Session["UserCode"] as String ?? "";
                string billingCode = Session["BillingCode"] as String ?? "";
                string relationship = Session["Relationship"] as String ?? "";

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

                    var fields = BRBFunctions_CSharp.GetProfile(userCode, billingCode);

                    if (fields.Count > 0)
                    {
                        UserIDCode1.Text = fields.GetStringValue("UserCode");
                        BillCode1.Text = fields.GetStringValue("BillingCode");
                        FullName1.Text = fields.GetStringValue("FullName");
                        MailAddress1.Text = fields.GetStringValue("MailAddress");
                        EmailAddress1.Text = fields.GetStringValue("Email");
                        PhoneNo1.Text = fields.GetStringValue("Phone");
                        Quest1.Text = fields.GetStringValue("Question1");
                        Quest2.Text = fields.GetStringValue("Question2");
                        FullName1.Text = fields.GetStringValue("FullName");
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