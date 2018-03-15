﻿using System;

namespace BRBPortal_CSharp
{
    public partial class Home : System.Web.UI.Page
    {
        public string BillingCode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string userCode = Session["UserCode"] as String;
            string billingCode = Session["BillingCode"] as String;

            if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
            {
                Response.Redirect("~/Account/Login");
            }
            else
            {
                this.BillingCode = userCode; // looks wrong, but that's what VB.NET code has...
            }
        }
    }
}