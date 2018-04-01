using BRBPortal_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class UpdateTenancy : System.Web.UI.Page
    {
        public List<BRBTenant> Tenants = null;
        public string DelimitedTenants = ""; // updated when tenants edited client-side
        public string RemovedTenantIDs = ""; // updated when existing tenants Removed client-side

        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Master.User;
            var property = user.CurrentProperty;
            var unit = user.CurrentUnit;

            Tenants = unit.Tenants;

            MainAddress.Text = property.PropertyAddress;
            UnitNo.Text = unit.UnitNo;
            OwnerName.Text = property.OwnerContactName;
            AgentName.Text = property.AgencyName;
            BalAmt.Text = property.Balance.ToString("C");
            UnitStatus.Text = unit.ClientPortalUnitStatusCode;
            InitRent.Text = unit.InitialRent;

            if (unit.StartDt.HasValue)
            {
                TenStDt.Text = unit.StartDt.Value.ToString("MM/dd/yyyy");
            }

            //HServs
            HServOthrBox.Text = "";
            NumTenants.Text = unit.TenantCount.ToString();

            RB1.SelectedValue = unit.SmokingProhibitionInLeaseStatus;

            if (unit.SmokingProhibitionEffectiveDate.HasValue)
            {
                SmokeDt.Text = unit.SmokingProhibitionEffectiveDate.Value.ToString("yyyy-MM-dd");
            }
            
            if (unit.DatePriorTenancyEnded.HasValue)
            {
                PTenDt.Text = unit.DatePriorTenancyEnded.Value.ToString("MM/dd/yyyy");
            }

            if (!string.IsNullOrEmpty(unit.ReasonPriorTenancyEnded))
            {
                var item = TermReas.Items.FindByText(unit.ReasonPriorTenancyEnded);

                if (item == null)
                {
                    TermReas.SelectedValue = unit.ReasonPriorTenancyEnded;
                }
                else
                {
                    TermDescr.Text = unit.ReasonPriorTenancyEnded;
                }
            }

            if (string.IsNullOrEmpty(AgentName.Text))
            {
                AgencyNameSection.Visible = false;
            }
        }

        protected void UpdateTenancy_Click(object sender, EventArgs e)
        {

        }
    }
}