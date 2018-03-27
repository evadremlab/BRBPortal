using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Runtime.Caching;
using PaymentReceiver.Models;

namespace PaymentReceiver.Controllers
{
    public class ValuesController : ApiController
    {
        const string CacheName = "Payments";
        ObjectCache cache = MemoryCache.Default;

        private List<Payment> GetPayments()
        {
            var payments = cache.Get(CacheName) as List<Payment>;

            if (payments == null)
            {
                payments = new List<Payment>();
            }

            return payments;
        }

        private void AddPayment(string xmlString)
        {
            var payments = GetPayments();

            payments.Add(new Payment
            {
                Timestamp = DateTime.Now,
                XmlString = xmlString
            });

            cache.Add(CacheName, payments, new CacheItemPolicy { SlidingExpiration = new TimeSpan(1, 0, 0, 0, 0) });
        }

        // GET api/values
        public List<Payment> Get()
        {
            return GetPayments();
        }

        // POST api/values
        public string Post(HttpRequestMessage request)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(request.Content.ReadAsStreamAsync().Result);

            var str = xmlDoc.InnerXml;

            AddPayment(str);

            return str;
        }
    }
}
