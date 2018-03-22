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
        private string iUnitNum = "";
        private string iUnitID = "";

        private DataTable iTenants = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            var userCode = Session["UserCode"] as String ?? "";
            var billingCode = Session["BillingCode"] as String ?? "";

            iUnitID = Session["UnitID"] as String ?? "";
            iUnitNum = Session["UnitNo"] as String ?? "";

            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }

            if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(billingCode))
            {
                Response.Redirect("~/Account/Login");
            }

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

        }
        private void LoadData()
        {
            var userCode = Session["UserCode"] as String ?? "";
            var billingCode = Session["BillingCode"] as String ?? "";
            var propertyID = Session["PropertyID"] as String ?? "";
            var propertyAddress = Session["PropAddr"] as String ?? "";
            var propertyBalance = Session["PropBalance"] as String ?? "";

            var fields = BRBFunctions_CSharp.GetPropertyTenants(propertyID, userCode, billingCode, iUnitID);

            if (fields.Count == 0)
            {
                if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                {
                    BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                }

                ShowDialogOK("Error retrieving Tenants: " + BRBFunctions_CSharp.iErrMsg, "View Tenants");
                return;
            }

            BRBFunctions_CSharp.iTenantsTbl.DefaultView.Sort = "LastName, FirstName ASC";
            BRBFunctions_CSharp.iTenantsTbl = BRBFunctions_CSharp.iTenantsTbl.DefaultView.ToTable();

            gvTenants.DataSource = BRBFunctions_CSharp.iTenantsTbl;
            gvTenants.DataBind();

            Session["TenantsTbl"] = BRBFunctions_CSharp.iTenantsTbl;

            MainAddress.Text = BRBFunctions_CSharp.iPropAddr;
            UnitNo.Text = iUnitNum;
            BalAmt.Text = propertyBalance;

            UnitStat.Text = fields.GetStringValue("CPStatus");
            HouseServs.Text = fields.GetStringValue("HServices");
            TenStDt.Text = fields.GetStringValue("StartDt");
            NumTenants.Text = fields.GetStringValue("NumTenants");
            SmokYN.Text = fields.GetStringValue("SmokeYN");
            SmokDt.Text = fields.GetStringValue("SmokeDt");
            InitRent.Text = fields.GetStringValue("InitRent");
            PriorEndDt.Text = fields.GetStringValue("PriorEndDt");
            TermReason.Text = fields.GetStringValue("TermReason");
            OwnerName.Text = fields.GetStringValue("OwnerName");
            AgentName.Text = fields.GetStringValue("AgenntName");

            FailureText.Text = "";
            ErrorMessage.Visible = false;
        }

        protected void ToUnits_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyProperties/MyUnits", false);
        }

        protected void gvTenants_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTenants.PageIndex = e.NewPageIndex;
            gvTenants.DataSource = Session["PropertyTbl"];
            gvTenants.DataBind();
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}