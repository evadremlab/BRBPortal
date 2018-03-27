using System;
using System.Data;
using System.Web;

/// <summary>
/// NOT USED, YET
/// </summary>
namespace BRBPortal_CSharp.Models
{
    public class UserSession
    {
        public string UserCode { get; set; } = "";
        public string BillingCode { get; set; } = "";
        public string Relationship { get; set; } = "";

        public string NextPage { get; set; }

        public string CartString { get; set; } // <= Cart
        public DataTable CartData { get; set; } // <= CartTbl

        public string PropertyID { get; set; }
        public string UnitID { get; set; }
        public string UnitNumber { get; set; } // <= UnitNo
        public string PropertyAddress { get; set; } // <= PropAddr
        public string PropertyBalance { get; set; } // <= PropBalance
        public string FeesAll { get; set; }

        public DataTable PropertiesData { get; set; } // <= PropertyTbl
        public DataTable TenantsData { get; set; } // <= TenantsTbl
        public DataTable UnitsTable { get; set; } // <= UnitsTbl

        public bool UpdateTenants { get; set; } // <= UpdTenants
        public bool UpdateUnitInfo { get; set; } // <= UpdUnitInfo

        public UserSession()
        {
            var httpSession = HttpContext.Current.Session; // alias for less typing :-)

            this.UserCode = httpSession["UserCode"] as String ?? "";
            this.BillingCode = httpSession["BillingCode"] as String ?? "";
            this.Relationship = httpSession["Relationship"] as String ?? "";
            this.NextPage = httpSession["NextPage"] as String ?? "";
            this.CartString = httpSession["CartString"] as String ?? "";

            this.PropertyID = httpSession["PropertyID"] as String ?? "";
            this.UnitID = httpSession["UnitID"] as String ?? "";
            this.UnitNumber = httpSession["UnitNumber"] as String ?? "";
            this.PropertyAddress = httpSession["PropertyAddress"] as String ?? "";
            this.PropertyBalance = httpSession["PropertyBalance"] as String ?? "";
            this.FeesAll = httpSession["FeesAll"] as String ?? "";
        }
    }
}