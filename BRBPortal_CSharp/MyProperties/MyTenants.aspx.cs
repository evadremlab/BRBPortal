using BRBPortal_CSharp.Models;
using System;
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
        private DataTable iTenants = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
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
            Response.Redirect("~/MyProperties/UpdateTenancy");
        }

        private void LoadData()
        {
            var user = Master.User;

            BRBFunctions_CSharp.GetPropertyTenants(ref user);

            if (user.CurrentProperty.Tenants.Count == 0)
            {
                if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                {
                    BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                }

                Master.ShowDialogOK("Error retrieving Tenants: " + BRBFunctions_CSharp.iErrMsg, "View Tenants");
                return;
            }

            var dataTable = BRBFunctions_CSharp.ConvertToDataTable<BRBTenant>(user.CurrentProperty.Tenants);
            gvTenants.DataSource = dataTable;
            gvTenants.DataBind();

            Master.UpdateSession(user);

            MainAddress.Text = user.CurrentProperty.PropertyAddress;
            UnitNo.Text = user.CurrentUnit.UnitNo;
            BalAmt.Text = user.CurrentProperty.Balance.ToString("C");

            UnitStat.Text = user.CurrentProperty.Units[0].ClientPortalUnitStatusCode;
            HouseServs.Text = user.CurrentUnit.HServices;

            if (user.CurrentUnit.StartDt.HasValue)
            {
                TenStDt.Text = user.CurrentUnit.StartDt.Value.ToString("MM/dd/yyyy");
            }

            //NumTenants.Text = user.CurrentUnit.NumberOfTenants.ToString();
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