using System;
using System.Web.UI;

namespace BRBPortal_CSharp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("https://rentportaldev.cityofberkeley.info/account/login.aspx");
        }
    }
}