using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.DAL;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.Account
{
    public partial class ManageSecurityQuestions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Submit_Click(object sender, EventArgs e)
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
                    Master.ShowErrorModal("Error updating confirmation (Error).", "Update Security Questions");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("ProfileConfirm", ex);
                Master.ShowErrorModal("Error updating confirmation (Exception).", "Confirm User Profile");
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ProfileList", false);
        }
    }
}