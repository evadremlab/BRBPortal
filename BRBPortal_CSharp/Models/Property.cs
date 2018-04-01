using System;
using System.Collections.Generic;

namespace BRBPortal_CSharp.Models
{
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
}