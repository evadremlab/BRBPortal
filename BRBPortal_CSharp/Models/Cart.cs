using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRBPortal_CSharp.Models
{
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
}