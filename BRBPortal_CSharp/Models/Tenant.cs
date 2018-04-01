using System;

namespace BRBPortal_CSharp.Models
{
    public class BRBTenant
    {
        public string TenantID { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";

        public string DisplayName
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
            }
        }
    }
}