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
                string userCode = Session["UserCode"] as String ?? "";
                string billingCode = Session["BillingCode"] as String ?? "";
                string relationship = Session["Relationship"] as String ?? "";

                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
                {
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    var fields = BRBFunctions_CSharp.GetProfile(userCode, billingCode);

                    if (fields.Count > 0)
                    {
                        UserIDCode2.Text = fields.GetStringValue("UserCode").Trim();
                        BillCode2.Text = fields.GetStringValue("BillingCode").Trim();
                        FirstName.Text = fields.GetStringValue("FirstName").Trim();
                        MidName.Text = fields.GetStringValue("MidName");
                        LastName.Text = fields.GetStringValue("LastName");
                        Suffix.Text = fields.GetStringValue("Suffix");
                        StNum.Text = fields.GetStringValue("StNum");
                        StName.Text = fields.GetStringValue("StName");
                        StUnit.Text = fields.GetStringValue("Unit");
                        StCity.Text = fields.GetStringValue("City");
                        StState.Text = fields.GetStringValue("State");
                        StZip.Text = fields.GetStringValue("Zip");
                        StCountry.Text = fields.GetStringValue("Country");
                        EmailAddress.Text = fields.GetStringValue("Email");
                        PhoneNo.Text = fields.GetStringValue("Phone");
                        Quest1.Text = fields.GetStringValue("Question1");
                        Answer1.Text = fields.GetStringValue("Answer1");
                        Quest2.Text = fields.GetStringValue("Question2");
                        Answer2.Text = fields.GetStringValue("Answer2");
                        AgencyName.Text = fields.GetStringValue("AgencyName");

                        if (relationship.ToUpper().Equals("OWNER"))
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
        }

        protected void UpdateProfile_Click(object sender, EventArgs e)
        {
            string relationship = Session["Relationship"] as String ?? "";

            if (relationship.ToUpper().Equals("OWNER"))
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

            var profile = new UserProfile
            {
                UserCode = UserIDCode2.Text,
                BillingCode = BillCode2.Text,
                FirstName = FirstName.Text,
                MiddleName = MidName.Text,
                LastName = LastName.Text,
                Suffix = Suffix.Text,
                StreetNumber = StNum.Text,
                StreetName = StName.Text,
                Unit = StUnit.Text,
                City = StCity.Text,
                Country = StCity.Text,
                Zip = StZip.Text,
                Email = EmailAddress.Text,
                PhoneNo = PhoneNo.Text,
                Question1 = Quest1.Text,
                Answer1 = Answer1.Text,
                Question2 = Quest2.Text,
                Answer2 = Answer2.Text,
                AgencyName = AgencyName.Text
            };

            profile.FullAddress = string.Format("{0} {1} {2}", profile.StreetNumber, profile.StreetName, profile.Unit);

            if (StState.SelectedIndex > -1)
            {
                profile.State = StState.Text;
            }

            if (BRBFunctions_CSharp.UpdateProfile(profile) == false)
            {
                ShowDialogOK("Error: Problem updating user profile.", "Profile Update");
                return;
            }

            Response.Redirect("~/Account/ProfileList.aspx", false);
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