﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyTenants : System.Web.UI.Page
    {
        private string iUnitNum = "";
        private string iUnitID = "";

        private DataTable iTenants = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            iUnitID = Session["UnitID"] as String ?? "";
            iUnitNum = Session["UnitNo"] as String ?? "";

            if (IsPostBack)
            {
                UpdateData();
            }
            else
            {
                LoadData();
            }
        }

        private void UpdateData()
        {
            Response.Redirect("~/MyProperties/UpdateTenancy.aspx");
        }

        private void LoadData()
        {
            var propertyID = Session["PropertyID"] as String ?? "";
            var propertyAddress = Session["PropAddr"] as String ?? "";
            var propertyBalance = Session["PropBalance"] as String ?? "";

            var user = Master.User;
            BRBFunctions_CSharp.GetPropertyTenants(ref user, propertyID, iUnitID);

            if (user.CurrentUnit.Tenants.Count == 0)
            {
                if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                {
                    BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                }

                Master.ShowDialogOK("Error retrieving Tenants: " + BRBFunctions_CSharp.iErrMsg, "View Tenants");
                return;
            }

            //BRBFunctions_CSharp.iTenantsTbl.DefaultView.Sort = "LastName, FirstName ASC";
            //BRBFunctions_CSharp.iTenantsTbl = BRBFunctions_CSharp.iTenantsTbl.DefaultView.ToTable();

            //gvTenants.DataSource = BRBFunctions_CSharp.iTenantsTbl;
            //gvTenants.DataBind();

            //Session["TenantsTbl"] = BRBFunctions_CSharp.iTenantsTbl;
            Master.UpdateSession(user);

            MainAddress.Text = user.CurrentProperty.MainStreetAddress;
            UnitNo.Text = iUnitNum;
            BalAmt.Text = propertyBalance;

            UnitStat.Text = user.CurrentProperty.Units[0].ClientPortalUnitStatusCode;
            HouseServs.Text = user.CurrentUnit.HServices;

            if (user.CurrentUnit.StartDt.HasValue)
            {
                TenStDt.Text = user.CurrentUnit.StartDt.Value.ToString("MM/dd/yyyy");
            }

            NumTenants.Text = user.CurrentUnit.NumberOfTenants.ToString();
            SmokYN.Text = user.CurrentUnit.SmokingProhibitionInLeaseStatus;

            if (user.CurrentUnit.SmokingProhibitionEffectiveDate.HasValue)
            {
                SmokDt.Text = user.CurrentUnit.SmokingProhibitionEffectiveDate.Value.ToString("MM/dd/yyyy");
            }

            InitRent.Text = user.CurrentUnit.InitialRent;

            if (user.CurrentUnit.DatePriorTenancyEnded.HasValue)
            {
                PriorEndDt.Text = user.CurrentUnit.DatePriorTenancyEnded.Value.ToString("MM/dd/yyyy");
            }

            TermReason.Text = user.CurrentUnit.TerminationReason;
            OwnerName.Text = user.CurrentProperty.OwnerContactName;
            AgentName.Text = user.CurrentProperty.AgencyName;
        }

        protected void gvTenants_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTenants.PageIndex = e.NewPageIndex;
            gvTenants.DataSource = Session["PropertyTbl"];
            gvTenants.DataBind();
        }
    }
}