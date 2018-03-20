using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.Account
{
    public partial class ProfileConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
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
                    UserIDCode0.Text = "";
                    BillCode0.Text = "";
                    FullName0.Text = "";
                    MailAddress0.Text = "";
                    EmailAddress0.Text = "";
                    PhoneNo0.Text = "";

                    var fields = BRBFunctions_CSharp.GetProfile(userCode, billingCode);

                    if (fields.Count > 0)
                    {
                        UserIDCode0.Text = fields.GetStringValue("UserCode");
                        BillCode0.Text = fields.GetStringValue("BillingCode");
                        FullName0.Text = fields.GetStringValue("FullName");
                        MailAddress0.Text = fields.GetStringValue("MailAddress");
                        EmailAddress0.Text = fields.GetStringValue("Email");
                        PhoneNo0.Text = fields.GetStringValue("Phone");
                        FullName0.Text = fields.GetStringValue("FullName");
                    }
                }

                btnSubmit.Enabled = false;
            }
        }

        protected void SubmitProfile_Click(object sender, EventArgs e)
        {
            var success = BRBFunctions_CSharp.ConfirmProfile(UserIDCode0.Text, BillCode0.Text, DeclareInits.Text);

            if (!success)
            {
                ShowDialogOK("Error updating confirmation.", "Confirm Profile");
            }

            Response.Redirect("~/Home", false);
        }

        protected void CancelProfile_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Account/Login", false);
        }

        protected void ShowDialogOK(string aMessage, string aTitle = "Status")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopupOK('" + aMessage + "', '" + aTitle + "');", true);
        }
    }
}