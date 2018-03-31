﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

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

    public class BRBCart
    {
        public uint? ID { get; set; }
        public bool WasUpdated { get; set; } = false;
        public decimal PaymentAmount { get; set; }
        public List<BRBCartItem> Items { get; set; } = new List<BRBCartItem>();
        public uint PaymentConfirmationNo { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentReceivedAmt { get; set; }
        public bool isFeeOnlyPaid { get; set; }
    }

    public class BRBCartItem
    {
        public int ID { get; set; }
        public string PropertyId { get; set; } = "";
        public string PropertyAddress { get; set; } = "";
        public decimal CurrentFee { get; set; }
        public decimal PriorFee { get; set; }
        public decimal CurrentPenalty { get; set; }
        public decimal PriorPenalty { get; set; }
        public decimal Credits { get; set; }
        public decimal Balance { get; set; }

        public decimal SumCurrentFees()
        {
            return this.CurrentFee + this.PriorFee;
        }

        public decimal SumPenalties(bool isFeeOnlyPaid)
        {

            if (isFeeOnlyPaid)
            {
                return 0;
            }
            else
            {
                return this.CurrentPenalty + this.PriorPenalty;
            }
        }

        public decimal SumBalance(bool isFeeOnlyPaid)
        {
            return this.SumCurrentFees() + this.SumPenalties(isFeeOnlyPaid);
        }
    }

    public class BRBProperty
    {
        public string PropertyID { get; set; } = "";
        public string OwnerContactName { get; set; } = "";
        public string AgencyName { get; set; } = "";
        public string PropertyAddress { get; set; } = "";
        public string BillingAddress { get; set; } = "";
        public string MainStreetAddress { get; set; } = "";
        public decimal CurrentFee { get; set; }
        public decimal PriorFee { get; set; }
        public decimal CurrentPenalty { get; set; }
        public decimal PriorPenalty { get; set; }
        public decimal Credits { get; set; }
        public decimal Balance { get; set; }
        public string BillingEmail { get; set; } = "";

        public List<BRBUnit> Units { get; set; } = new List<BRBUnit>();
        public List<BRBTenant> Tenants { get; set; } = new List<BRBTenant>();

        public BRBCartItem ConvertToCartItem()
        {
            return new BRBCartItem
            {
                PropertyId = this.PropertyID,
                PropertyAddress = this.PropertyAddress,
                Balance = this.Balance,
                Credits = this.Credits,
                CurrentFee = this.CurrentFee,
                CurrentPenalty = this.CurrentPenalty,
                PriorFee = this.PriorFee,
                PriorPenalty = this.PriorPenalty,
            };
        }
    }

    public class BRBUnit
    {
        public string UnitID { get; set; } = "";
        public string UnitNo { get; set; } = "";
        public string UnitStatID { get; set; } = "";
        public string UnitStatCode { get; set; } = "";
        public string StreetAddress { get; set; } = "";
        public string ClientPortalUnitStatusCode { get; set; } = "";
        public decimal RentCeiling { get; set; }
        public DateTime? StartDt { get; set; }
        public string HServices { get; set; } = "";
        public string CPUnitStatDisp { get; set; } = "";
        public string ExemptionReason { get; set; } = "";
        public string OtherExemptionReason { get; set; } = "";
        public string OccupiedBy { get; set; } = "";
        public int TenantCount { get; set; } // readonly
        public string SmokingProhibitionInLeaseStatus { get; set; } = "";
        public DateTime? SmokingProhibitionEffectiveDate { get; set; }
        public string SmokeDetector { get; set; } = "";
        public string InitialRent { get; set; } = "";
        public string PriorEndDate { get; set; } = "";
        public string TerminationReason { get; set; } = "";
        public DateTime? DatePriorTenancyEnded { get; set; }
        public string ReasonPriorTenancyEnded { get; set; }
        public DateTime? UnitAsOfDt { get; set; }
        public string ContractNo { get; set; }
        public string CommUseDesc { get; set; }
        public string CommResYN { get; set; }
        public string CommZoneUse { get; set; }
        public string PropMgrName { get; set; }
        public string PMEmailPhone { get; set; }
        public string PrincResYN { get; set; }
        public string MultiUnitYN { get; set; }
        public string OtherUnits { get; set; }
        public string DeclarationInitials { get; set; }
        public string TenantNames { get; set; }
        public string TenantContacts { get; set; }

        public List<BRBTenant> Tenants { get; set; } = new List<BRBTenant>();
    }

    public class BRBTenant
    {
        public string TenantID { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string UnitStatusCode { get; set; } = "";
        public string HousingServices { get; set; } = "";
        public DateTime? StartDate { get; set; }
        //public string OwnerName { get; set; } = "";
        //public string AgentName { get; set; } = "";
        //public string UnitID { get; set; } = "";
        //public string OwnerEmail { get; set; } = "";
    }
}