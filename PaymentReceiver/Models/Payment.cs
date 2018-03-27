using System;

namespace PaymentReceiver.Models
{
    public class Payment
    {
        public DateTime Timestamp { get; set; }
        public string XmlString { get; set; }
    }
}