using System;
using System.Data;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyTenants : System.Web.UI.Page
    {
        private DataTable iTenants = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = Master.User;
                var provider = Master.DataProvider;

                if (provider.GetUnitTenants(ref user))
                {
                    var property = user.CurrentProperty;
                    var unit = user.CurrentUnit;
                    var tenants = unit.Tenants;

                    Master.UpdateSession(user);

                    hdnNumTenants.Value = tenants.Count.ToString();

                    MainAddress.Text = property.PropertyAddress;
                    UnitNo.Text = unit.UnitNo;
                    BalAmt.Text = property.Balance.ToString("C");

                    UnitStat.Text = unit.ClientPortalUnitStatusCode;
                    HServices.Text = unit.HServices;

                    if (unit.TenancyStartDate.HasValue)
                    {
                        TenStDt.Text = unit.TenancyStartDate.Value.ConvertForLiteral();
                    }

                    NumTenants.Text = unit.TenantCount.ToString();
                    SmokYN.Text = unit.SmokingProhibitionInLeaseStatus;

                    if (unit.SmokingProhibitionEffectiveDate.HasValue)
                    {
                        SmokDt.Text = unit.SmokingProhibitionEffectiveDate.Value.ConvertForLiteral();
                    }

                    InitRent.Text = unit.InitialRent;

                    if (unit.DatePriorTenancyEnded.HasValue)
                    {
                        PriorEndDt.Text = unit.DatePriorTenancyEnded.Value.ConvertForLiteral();
                    }

                    TermReason.Text = unit.ReasonPriorTenancyEnded;
                    OwnerName.Text = property.OwnerContactName;
                    AgentName.Text = property.AgencyName;

                    BindTenantsGridView();
                }
                else
                {
                    Master.ShowErrorModal("Error retrieving Tenants: " + provider.ErrorMessage, "View Tenants");
                }
            }
        }

        protected void gvTenants_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTenants.PageIndex = e.NewPageIndex;
            BindTenantsGridView();
        }

        private void BindTenantsGridView()
        {
            var user = Master.User;
            var provider = Master.DataProvider;

            gvTenants.DataSource = provider.ConvertToDataTable<BRBTenant>(user.CurrentUnit.Tenants);
            gvTenants.DataBind();
        }
    }
}