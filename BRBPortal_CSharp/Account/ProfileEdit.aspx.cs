using BRBPortal_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.Account
{
    public partial class ProfileEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = Master.User;

                if (BRBFunctions_CSharp.GetProfile(ref user))
                {
                    Master.UpdateSession(user);

                    UserIDCode2.Text = Master.User.UserCode;
                    BillCode2.Text = Master.User.BillingCode;
                    FirstName.Text = Master.User.FirstName;
                    MidName.Text = Master.User.MiddleName;
                    LastName.Text = Master.User.LastName;
                    Suffix.Text = Master.User.Suffix;
                    StNum.Text = Master.User.StreetNumber;
                    StName.Text = Master.User.StreetName;
                    StUnit.Text = Master.User.UnitNumber;
                    StCity.Text = Master.User.City;
                    StState.Text = Master.User.StateCode;
                    StZip.Text = Master.User.ZipCode;
                    StCountry.Text = Master.User.Country;
                    EmailAddress.Text = Master.User.Email;
                    PhoneNo.Text = Master.User.PhoneNumber;
                    AgencyName.Text = Master.User.AgencyName;

                    if (Master.User.Relationship.ToUpper().Equals("OWNER"))
                    {
                        Relationship.Text = "Owner";
                        OwnerGrp.Visible = true;
                        AgencyGrp.Visible = false;
                    }
                    else
                    {
                        Relationship.Text = "Agent";
                        OwnerGrp.Visible = false;
                        AgencyGrp.Visible = true;
                    }
                }
                else
                {
                    OwnerGrp.Visible = true;
                    AgencyGrp.Visible = false;
                }
            }
        }

        protected void UpdateProfile_Click(object sender, EventArgs e)
        {
            if (Master.User.Relationship.ToUpper().Equals("OWNER"))
            {
                if (string.IsNullOrEmpty(FirstName.Text) || string.IsNullOrEmpty(LastName.Text))
                {
                    ShowDialogOK("When Relationship is Owner, first and last name must be entered.");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(AgencyName.Text))
                {
                    ShowDialogOK("When Relationship is Agent, agency name must be entered.");
                    return;
                }
            }

            if (StState.SelectedIndex > -1)
            {
                Master.User.StateCode = StState.Text;
            }

            var user = Master.User;

            if (BRBFunctions_CSharp.UpdateProfile(ref user))
            {
                Master.UpdateSession(user);

                Response.Redirect("~/Account/ProfileList.aspx", false);
            }
            else
            {
                ShowDialogOK("Error: Problem updating user profile.", "Profile Update");
            }
        }

        protected void CancelEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ProfileList.aspx", false);
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}