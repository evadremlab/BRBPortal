using System;
using System.Collections.Generic;

namespace BRBPortal_CSharp.Models
{
    public class BRBUser
    {
        public string UserCode { get; set; } = "";
        public string BillingCode { get; set; } = "";
        public string Relationship { get; set; } = "";
        public bool IsFirstlogin { get; set; } = false;
        public bool IsTemporaryPassword { get; set; } = false;

        #region Owner Fields
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Suffix { get; set; } = "";
        public string FullName
        {
            get
            {
                return string.Format("{0}{1} {2} {3}",
                    this.FirstName,
                    string.IsNullOrEmpty(this.MiddleName) ? "" : (" " + this.MiddleName),
                    this.LastName,
                    this.Suffix
                );
            }
        }
        #endregion

        #region Agent Fields
        public string AgencyName { get; set; } = "";
        public string PropertyOwnerLastName { get; set; } = "";
        #endregion

        public string StreetNumber { get; set; } = "";
        public string StreetName { get; set; } = "";
        public string UnitNumber { get; set; } = "";
        public string City { get; set; } = "";
        public string StateCode { get; set; } = "";
        public string Country { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public string MailAddress
        {
            get
            {
                return string.Format(" {0}, {1}{2}, {3} {4}",
                    this.StreetName,
                    this.StreetNumber,
                    string.IsNullOrEmpty(this.UnitNumber) ? "" : (", " + this.UnitNumber),
                    this.City,
                    this.StateCode,
                    this.ZipCode).EscapeXMLChars();
            }
        }
        public string FullAddress { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";

        public string DeclarationInitials { get; set; } = "";
        public string PropertyAddress { get; set; } = "";

        public string Question1 { get; set; } = "";
        public string Answer1 { get; set; } = "";
        public string Question2 { get; set; } = "";
        public string Answer2 { get; set; } = "";

        public BRBUnit CurrentUnit { get; set; }
        public BRBProperty CurrentProperty { get; set; }

        public string FeesAll { get; set; } = "";
        public BRBCart Cart { get; set; } = new BRBCart();
        public List<BRBProperty> Properties { get; set; } = new List<BRBProperty>();
    }
}