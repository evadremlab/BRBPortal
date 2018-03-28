using System;

namespace RentPaymentReceiver.Models
{
    public class Payment
    {
        public DateTime Timestamp { get; set; }
        public string XmlString { get; set; }
    }
}