using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BRBPortal_CSharp
{
    public class DataManager
    {
        private Dictionary<string, string> Fields;
        private string[] fieldDelimiter = new string[] { "::" };

        const string SOAP_NAMESPACE = "http://cityofberkeley.info/RTS/ClientPortal/API";
        const string URI_PREFIX = "http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.";

        public DataManager()
        {
            Fields = new Dictionary<string, string>();
        }

        private static StringBuilder NewSoapMessage()
        {
            return new StringBuilder(string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""{0}"">", SOAP_NAMESPACE));
        }

        public Dictionary<string, string> Parse(string soapString)
        {
            Fields = new Dictionary<string, string>();

            foreach (var item in soapString.Split(fieldDelimiter, StringSplitOptions.None))
            {
                var parts = item.Split('=');
                var key = parts[0];
                var value = parts[1];

                if (!Fields.ContainsKey(key))
                {
                    Fields.Add(key, value);
                }
            }

            return Fields;
        }

        public string GetField(string key)
        {
            return Fields.ContainsKey(key) ? Fields[key] : string.Empty;
        }
    }
}