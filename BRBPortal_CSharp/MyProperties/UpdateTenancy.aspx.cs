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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddTenant_Click(object sender, EventArgs e)
        {

        }

        protected void SaveNewTenant_Click(object sender, EventArgs e)
        {

        }

        protected void CancelNewTenant_Click(object sender, EventArgs e)
        {

        }

        protected void UpdateTenancy_Click(object sender, EventArgs e)
        {

        }

        protected void CancelEdit_Click(object sender, EventArgs e)
        {

        }

        protected void gvTenants_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTenants.PageIndex = e.NewPageIndex;
            gvTenants.DataSource = Session["TenantsTbl"];
            gvTenants.DataBind();
        }
    }
}