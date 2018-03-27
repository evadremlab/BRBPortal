﻿using System;

using BRBPortal_CSharp.Models;

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
                            ShowDialogOK("Last name must be entered when property relationship is Owner.");
                            return;
                        }
                    }
                    else
                    {
                        if (AgencyName.Text == "")
                        {
                            ShowDialogOK("Agency name must be entered when property relationship is Agent.");
                            return;
                        }
                    }

                    var profile = new UserProfile
                    {
                        UserCode = ReqUserID.Text.ToUpper(),
                        BillingCode = BillCode.Text.ToUpper(),
                        Relationship = PropRelate.Text, // Owner or Agency

                        // Owner fields
                        LastName = LastName.Text.ToUpper(),

                        // Agency fields
                        AgencyName = AgencyName.Text.ToUpper(),
                        PropertyOwnerLastName = PropOwnLastName.Text.ToUpper(),

                        StreetNumber = StNum.Text.ToUpper(),
                        StreetName = StName.Text.ToUpper(),
                        City = StCity.Text.ToUpper(),
                        State = StState.Text.ToUpper(),

                        Email = EmailAddress.Text.ToUpper(),
                        PhoneNo = PhoneNo.Text.ToUpper(),
                        PropertyAddress = PropAddress.Text.ToUpper()
                    };

                    if (BRBFunctions_CSharp.Register(profile) == false)
                    {
                        ShowDialogOK(BRBFunctions_CSharp.iErrMsg, "Registration Error");
                        return;
                    }

                    Response.Redirect("~/Account/Login.aspx", true);
                }
                catch (Exception ex)
                {
                    ShowDialogOK(ex.Message, "Register");
                }
            }
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}