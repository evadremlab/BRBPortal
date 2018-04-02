using Microsoft.AspNet.Identity;
using System;
using System.Web;
using BRBPortal_CSharp.DAL;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.Account
{
    public partial class ProfileConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (IsPostBack)
            {
                btnSubmit.Attributes.Remove("disabled");
                return;
            }

            btnSubmit.Attributes.Add("disabled", "disabled");

            var user = Master.User;
            var provider = Master.DataProvider;

            try
            {
                if (provider.GetUserProfile(ref user))
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
                else
                {
                    Logger.Log("ProfileConfirm", provider.ErrorMessage);
                    Master.ShowErrorModal("Error getting User profile.", "ProfileConfirm");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("ProfileConfirm", ex);
                Master.ShowErrorModal("Error getting User profile.", "ProfileConfirm");
            }
        }

        protected void SubmitProfile_Click(object sender, EventArgs e)
        {
            var user = Master.User;
            var provider = Master.DataProvider;

            user.Question1 = Quest1.Text;
            user.Answer1 = Answer1.Text;
            user.Question2 = Quest2.Text;
            user.Answer2 = Answer2.Text;
            user.DeclarationInitials = DeclareInits.Text;

            try
            {
                if (provider.ConfirmUserProfile(user))
                {
                    Session["ShowAfterRedirect"] = "Your account profile has been confirmed.|Profile Confirmed";

                    Response.Redirect("~/Home", false);
                }
                else
                {
                    Logger.Log("ProfileConfirm", provider.ErrorMessage);
                    Master.ShowErrorModal("Error updating confirmation (Error).", "Confirm User Profile");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("ProfileConfirm", ex);
                Master.ShowErrorModal("Error updating confirmation (Exception).", "Confirm User Profile");
            }
        }

        protected void CancelProfile_Click(object sender, EventArgs e)
        {
            try
            {
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                Session.RemoveAll();

                Response.Redirect("~/Account/Login", false);
            }
            catch (Exception ex)
            {
                Logger.LogException("ProfileConfirm", ex);
                Master.ShowErrorModal("Error when cancelling profile confirmation.", "Confirm User Profile");
            }
        }
    }
}