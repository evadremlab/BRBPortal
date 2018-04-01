using System;
using System.Data;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.DAL;
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
                var provider = new DataProvider();

                if (provider.GetUnitTenants(ref user))
                {
                    var property = user.CurrentProperty;
                    var unit = user.CurrentUnit;
                    var tenants = unit.Tenants;

                    Master.UpdateSession(user);

                    hdnNumTenants.Value = tenants.Count.ToString();

                    if (tenants.Count == 0)
                    {
                        tenants.Add(new BRBTenant
                        {
                            DisplayName = "no tenants"
                        });
                    }

                    MainAddress.Text = property.PropertyAddress;
                    UnitNo.Text = unit.UnitNo;
                    BalAmt.Text = property.Balance.ToString("C");

                    UnitStat.Text = unit.ClientPortalUnitStatusCode;
                    HouseServs.Text = unit.HServices;

                    if (unit.StartDt.HasValue)
                    {
                        TenStDt.Text = unit.StartDt.Value.ToString("MM/dd/yyyy");
                    }

                    NumTenants.Text = unit.TenantCount.ToString();
                    SmokYN.Text = unit.SmokingProhibitionInLeaseStatus;

                    if (unit.SmokingProhibitionEffectiveDate.HasValue)
                    {
                        SmokDt.Text = unit.SmokingProhibitionEffectiveDate.Value.ToString("MM/dd/yyyy");
                    }

                    InitRent.Text = unit.InitialRent;

                    if (unit.DatePriorTenancyEnded.HasValue)
                    {
                        PriorEndDt.Text = unit.DatePriorTenancyEnded.Value.ToString("MM/dd/yyyy");
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
            var provider = new DataProvider();

            gvTenants.DataSource = provider.ConvertToDataTable<BRBTenant>(user.CurrentUnit.Tenants);
            gvTenants.DataBind();
        }
    }
}