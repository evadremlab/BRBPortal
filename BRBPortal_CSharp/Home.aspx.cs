using System;

namespace BRBPortal_CSharp
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }
        }

        protected void MngSel_Click(object sender, EventArgs e)
        {
            if (HomeOption.SelectedValue == "Profile")
            {
                Response.Redirect("~/Account/ProfileList.aspx");
            }
            else if (HomeOption.SelectedValue == "Properties")
            {
                Response.Redirect("~/Properties/MyProperties.aspx");
            }
        }
    }
}