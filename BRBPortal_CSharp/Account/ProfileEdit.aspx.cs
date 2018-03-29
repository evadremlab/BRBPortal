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

                    UserIDCode2.Text = user.UserCode;
                    BillCode2.Text = user.BillingCode;
                    FirstName.Text = user.FirstName;
                    MidName.Text = user.MiddleName;
                    LastName.Text = user.LastName;
                    Suffix.Text = user.Suffix;
                    StNum.Text = user.StreetNumber;
                    StName.Text = user.StreetName;
                    StUnit.Text = user.UnitNumber;
                    StCity.Text = user.City;
                    StState.Text = user.StateCode;
                    StZip.Text = user.ZipCode;
                    StCountry.Text = user.Country;
                    EmailAddress.Text = user.Email;
                    PhoneNo.Text = user.PhoneNumber;
                    AgencyName.Text = user.AgencyName;

                    if (user.Relationship.ToUpper().Equals("OWNER"))
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
            var user = Master.User;

            if (user.Relationship.ToUpper().Equals("OWNER"))
            {
                if (string.IsNullOrEmpty(FirstName.Text) || string.IsNullOrEmpty(LastName.Text))
                {
                    Master.ShowDialogOK("When Relationship is Owner, first and last name must be entered.");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(AgencyName.Text))
                {
                    Master.ShowDialogOK("When Relationship is Agent, agency name must be entered.");
                    return;
                }
            }

            if (StState.SelectedIndex > -1)
            {
                user.StateCode = StState.Text;
            }

            user.FirstName = FirstName.Text.ToUpper();
            user.MiddleName = MidName.Text.ToUpper();
            user.LastName = LastName.Text.ToUpper();
            user.Suffix = Suffix.Text;
            user.StreetNumber = StNum.Text.ToUpper();
            user.StreetName = StName.Text.ToUpper();
            user.UnitNumber = StUnit.Text.ToUpper();
            user.City = StCity.Text.ToUpper();
            user.StateCode = StState.Text;
            user.ZipCode = StZip.Text.ToUpper();
            user.Country = StCountry.Text.ToUpper();
            user.Email = EmailAddress.Text.ToUpper();
            user.PhoneNumber = PhoneNo.Text;
            user.AgencyName = AgencyName.Text;

            if (BRBFunctions_CSharp.UpdateProfile(ref user))
            {
                Master.UpdateSession(user);

                Response.Redirect("~/Account/ProfileList", false);
            }
            else
            {
                Master.ShowDialogOK("Error: Problem updating user profile.", "Profile Update");
            }
        }

        protected void CancelEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ProfileList", false);
        }
    }
}