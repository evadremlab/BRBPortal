using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class UpdateUnit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MainAddress.Text = "address is tbd";
            UnitNo.Text = "123";
            UnitStatus.Text = "available";
            ExemptReas.Text = "unknown";
            UnitStartDt.Text = "12/31/2017";
            UnitOccBy.Text = "somebody";
            UnitAsOfDt.Text = "12/31/2017";
            StartDt.Text = "12/31/2017";
            OccupiedBy.Text = "unknown";
        }

        protected void UpdateUnit_Click(object sender, EventArgs e)
        {
        }

        protected void CancelEdit_Click(object sender, EventArgs e)
        {
        }
    }
}