using System;
using System.Web.UI.WebControls;
using BRBPortal_CSharp.DAL;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyProperties : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = Master.User;
                var provider = Master.DataProvider;

                if (provider.GetProperties(ref user))
                {
                    var properties = user.Properties;

                    Master.UpdateSession(user);

                    if (properties.Count == 0)
                    {
                        properties.Add(new BRBProperty
                        {
                            PropertyAddress = "no properties"
                        });
                    }

                    var dataTable = provider.ConvertToDataTable<BRBProperty>(properties);
                    dataTable.DefaultView.Sort = "PropertyAddress ASC";
                    gvProperties.DataSource = dataTable;
                    gvProperties.DataBind();
                }
                else
                {
                    Master.ShowErrorModal("Error retrieving Properties: " + provider.ErrorMessage, "Properties");
                    return;
                }
            }
        }

        protected void gvProperties_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            var user = Master.User;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvProperties.Rows[rowIndex];

            user.CurrentProperty = user.Properties[rowIndex];

            Master.UpdateSession(user);

            if (e.CommandName.Equals("Select"))
            {
                Response.Redirect("~/MyProperties/MyUnits", false);
            }
        }

        private decimal GetRowDecimalValue(GridViewRow row, int cellIndex, decimal defaultValue = 0.0M )
        {
            var value = defaultValue;

            Decimal.TryParse(row.Cells[cellIndex].Text.Replace("$", ""), out value);

            return value;
        }

        protected void gvProperties_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var user = Master.User;
            var provider = Master.DataProvider;
            var dataTable = provider.ConvertToDataTable<BRBProperty>(user.Properties);

            gvProperties.PageIndex = e.NewPageIndex;
            dataTable.DefaultView.Sort = "PropertyAddress ASC";
            gvProperties.DataSource = dataTable;
            gvProperties.DataBind();

        }
    }
}