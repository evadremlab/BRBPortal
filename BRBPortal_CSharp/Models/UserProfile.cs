using System;

namespace BRBPortal_CSharp.Models
{
    public class UserProfile
    {
        public string UserCode { get; set; }
        public string BillingCode { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string FullAddress { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }

        public string Question1 { get; set; }
        public string Answer1 { get; set; }
        public string Question2 { get; set; }
        public string Answer2 { get; set; }

        public string AgencyName { get; set; }
    }
}