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

            Tenants = user.CurrentProperty.Tenants;
        }

        protected void UpdateTenancy_Click(object sender, EventArgs e)
        {

        }
    }
}