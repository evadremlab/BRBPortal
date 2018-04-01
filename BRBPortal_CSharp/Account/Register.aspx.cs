using System;
using BRBPortal_CSharp.DAL;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                try
                {
                    if (PropRelate.SelectedValue.ToString() == "Owner")
                    {
                        if (string.IsNullOrEmpty(LastName.Text))
                        {
                            Master.ShowErrorModal("Last name must be entered when property relationship is Owner.");
                            return;
                        }
                    }
                    else
                    {
                        if (AgencyName.Text == "")
                        {
                            Master.ShowErrorModal("Agency name must be entered when property relationship is Agent.");
                            return;
                        }
                    }

                    var user = Master.User;
                    var provider = Master.DataProvider;

                    user.UserCode = ReqUserID.Text.ToUpper();
                    user.BillingCode = BillCode.Text.ToUpper();
                    user.Relationship = PropRelate.Text; // Owner or Agency

                    // Owner fields
                    user.LastName = LastName.Text.ToUpper();

                    // Agency fields
                    user.AgencyName = AgencyName.Text.ToUpper();
                    user.PropertyOwnerLastName = PropOwnLastName.Text.ToUpper();

                    user.StreetNumber = StNum.Text.ToUpper();
                    user.StreetName = StName.Text.ToUpper();
                    user.City = StCity.Text.ToUpper();
                    user.StateCode = StState.Text.ToUpper();

                    user.Email = EmailAddress.Text;
                    user.PhoneNumber = PhoneNo.Text.ToUpper();
                    user.PropertyAddress = PropAddress.Text.ToUpper();

                    if (provider.Register(ref user))
                    {
                        Master.UpdateSession(user);
                        Response.Redirect("~/Account/ManagePassword", false);
                    }
                    else
                    {
                        Logger.Log("Register", provider.ErrorMessage);
                        Master.ShowErrorModal("Error during registration(1).", "Registration");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException("Register", ex);
                    Master.ShowErrorModal("Error during registration(2).", "Registration");
                }
            }
        }
    }
}