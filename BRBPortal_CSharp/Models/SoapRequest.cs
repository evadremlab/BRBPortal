using System;
using System.Text;

namespace BRBPortal_CSharp.Models
{
    public class SoapRequest
    {
        private StringBuilder sb = new StringBuilder();

        public string Url { get; set; }
        public string Action { get; set; }
        public string StaticDataFile { get; set; }

        public StringBuilder Body {
            get { return sb; }
        }
    }
}