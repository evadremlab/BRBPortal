﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.Account
{
    public partial class ProfileList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                        UserIDCode1.Text = fields.GetStringValue("UserCode");
                        BillCode1.Text = fields.GetStringValue("BillingCode");
                        Relationship.Text = relationship;
                        FullName1.Text = fields.GetStringValue("FullName");
                        MailAddress1.Text = fields.GetStringValue("MailAddr");
                        EmailAddress1.Text = fields.GetStringValue("Email");
                        PhoneNo1.Text = fields.GetStringValue("Phone");
                        Quest1.Text = fields.GetStringValue("Question1");
                        Quest2.Text = fields.GetStringValue("Question2");
                    }
                }
            }
        }

        protected void EditProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ProfileEdit.aspx", false);
        }

        protected void CancelList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx", false);
        }
    }
}