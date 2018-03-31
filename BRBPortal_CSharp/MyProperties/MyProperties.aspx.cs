using BRBPortal_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace BRBPortal_CSharp.MyProperties
{
    public partial class MyProperties : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = Master.User;

                if (BRBFunctions_CSharp.GetUserProperties(ref user))
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

                    var dataTable = BRBFunctions_CSharp.ConvertToDataTable<BRBProperty>(properties);
                    dataTable.DefaultView.Sort = "PropertyAddress ASC";
                    gvProperties.DataSource = dataTable;
                    gvProperties.DataBind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(BRBFunctions_CSharp.iErrMsg))
                    {
                        if (BRBFunctions_CSharp.iErrMsg.IndexOf("(500) Internal Server Error") > -1)
                        {
                            BRBFunctions_CSharp.iErrMsg = "(500) Internal Server Error";
                        }

                        Master.ShowErrorModal("Error retrieving Properties: " + BRBFunctions_CSharp.iErrMsg, "Properties");
                        return;
                    }
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
            var dataTable = BRBFunctions_CSharp.ConvertToDataTable<BRBProperty>(user.Properties);

            gvProperties.PageIndex = e.NewPageIndex;
            dataTable.DefaultView.Sort = "PropertyAddress ASC";
            gvProperties.DataSource = dataTable;
            gvProperties.DataBind();

        }
    }
}