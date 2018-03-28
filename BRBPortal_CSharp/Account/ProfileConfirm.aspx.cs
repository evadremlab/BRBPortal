﻿using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;
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

                try
                {
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
                catch (Exception ex)
                {
                    Logger.LogException("ProfileConfirm", ex);
                    Master.ShowDialogOK("Error getting User profile.", "ProfileConfirm");
                }
            }
        }

        protected void SubmitProfile_Click(object sender, EventArgs e)
        {
            var user = Master.User;

            user.Question1 = Quest1.Text;
            user.Answer1 = Answer1.Text;
            user.Question2 = Quest2.Text;
            user.Answer2 = Answer2.Text;
            user.DeclarationInitials = DeclareInits.Text;

            try
            {
                if (BRBFunctions_CSharp.ConfirmProfile(user))
                {
                    Response.Redirect("~/Home");
                }
                else
                {
                    Logger.Log("ProfileConfirm", BRBFunctions_CSharp.iErrMsg);
                    Master.ShowDialogOK("Error updating confirmation.", "Confirm Profile");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("ProfileConfirm", ex);
                Master.ShowDialogOK("Error updating confirmation.", "Confirm Profile");
            }
        }

        protected void CancelProfile_Click(object sender, EventArgs e)
        {
            try
            {
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                Session.RemoveAll();

                Response.Redirect("~/Account/Login");
            }
            catch (Exception ex)
            {
                Logger.LogException("ProfileConfirm", ex);
                Master.ShowDialogOK("Error when cancelling profile confirmation.", "Confirm Profile");
            }
        }
    }
}