using BRBPortal_CSharp.Models;
using Microsoft.AspNet.Identity;
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
                var user = Master.User;

                if (BRBFunctions_CSharp.GetProfile(ref user))
                {
                    Master.UpdateSession(user);

                    UserIDCode0.Text = Master.User.UserCode;
                    BillCode0.Text = Master.User.BillingCode;
                    FullName0.Text = Master.User.FullName;
                    MailAddress0.Text = Master.User.MailAddress;
                    EmailAddress0.Text = Master.User.Email;
                    PhoneNo0.Text = Master.User.PhoneNumber;
                    FullName0.Text = Master.User.FullName;
                }
            }
        }

        protected void SubmitProfile_Click(object sender, EventArgs e)
        {
            Master.User.Question1 = Quest1.Text;
            Master.User.Answer1 = Answer1.Text;
            Master.User.Question2 = Quest2.Text;
            Master.User.Answer2 = Answer2.Text;
            Master.User.DeclarationInitials = DeclareInits.Text;

            var success = BRBFunctions_CSharp.ConfirmProfile(Master.User);

            if (!success)
            {
                Master.ShowDialogOK("Error updating confirmation.", "Confirm Profile");
            }

            Response.Redirect("~/Home", true);
        }

        protected void CancelProfile_Click(object sender, EventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            Session.RemoveAll();

            Response.Redirect("~/Account/Login", true);
        }
    }
}