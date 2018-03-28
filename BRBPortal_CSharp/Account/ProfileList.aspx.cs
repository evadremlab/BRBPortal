using System;

namespace BRBPortal_CSharp.Account
{
    public partial class ProfileList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = Master.User;

                if (BRBFunctions_CSharp.GetProfile(ref user))
                {
                    Master.UpdateSession(user);

                    UserIDCode1.Text = user.UserCode;
                    BillCode1.Text = user.BillingCode;
                    Relationship.Text = user.Relationship;
                    FullName1.Text = user.FullName;
                    MailAddress1.Text = user.MailAddress;
                    EmailAddress1.Text = user.Email;
                    PhoneNo1.Text = user.PhoneNumber;
                }
            }
        }

        protected void EditProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ProfileEdit");
        }

        protected void CancelList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home");
        }
    }
}