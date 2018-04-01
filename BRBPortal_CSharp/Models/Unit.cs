﻿using System;
using System.Collections.Generic;

namespace BRBPortal_CSharp.Models
{
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
        public string TerminationReason { get; set; } = "";
        public string OtherTerminationReason { get; set; } = "";
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
}