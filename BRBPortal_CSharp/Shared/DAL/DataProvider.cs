using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BRBPortal_CSharp.Models;
using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp.DAL
{
    public partial class DataProvider
    {
        private const bool USE_MOCK_SERVICES = false;

        public string Status = "";
        public string ErrorMessage = "";

        const string soapNamespace = "http://cityofberkeley.info/RTS/ClientPortal/API";
        const string urlPrefix = "http://cobwmisdv2.berkeley.root:5555/ws/RTSClientPortalAPI.API.WSD.";

        public DateTime? GetOptionalDate(string dateString)
        {
            DateTime dt = DateTime.MinValue;

            if (DateTime.TryParse(dateString, out dt))
            {
                return dt;
            }

            return null;
        }

        private StringBuilder NewSoapMessage()
        {
            return new StringBuilder(string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""{0}"">", soapNamespace));
        }


        private XmlDocument GetXmlResponse(SoapRequest soapRequest)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader reader = null;
            var xmlDoc = new XmlDocument();
            var soapMessage = NewSoapMessage();
            var xmlString = "";

            Status = "";
            ErrorMessage = "";

            try
            {
                if (USE_MOCK_SERVICES && !string.IsNullOrEmpty(soapRequest.StaticDataFile))
                {
                    xmlDoc = GetStaticXml(soapRequest.StaticDataFile);
                }
                else
                {
                    soapMessage.Append("<soapenv:Header/>");
                    soapMessage.Append("<soapenv:Body>");
                    soapMessage.Append(soapRequest.Body.ToString());
                    soapMessage.Append("</soapenv:Body>");
                    soapMessage.Append("</soapenv:Envelope>");

                    Logger.Log(string.Format("{0}(request)", soapRequest.Source), soapMessage.ToString());

                    var soapByte = System.Text.Encoding.UTF8.GetBytes(soapMessage.ToString());

                    Logger.Log("SoapRequest URL", urlPrefix + soapRequest.Url);

                    try
                    {
                        request = WebRequest.Create(urlPrefix + soapRequest.Url);
                        request.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                        request.Headers.Add("SOAPAction", soapRequest.Action);
                        request.ContentType = "text/xml; charset=utf-8";
                        request.ContentLength = soapByte.Length;
                        request.Method = "POST";

                        requestStream = request.GetRequestStream();
                        requestStream.Write(soapByte, 0, soapByte.Length);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    try
                    {
                        response = request.GetResponse();
                        responseStream = response.GetResponseStream();
                        reader = new StreamReader(responseStream);

                        xmlString = reader.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    Logger.Log(string.Format("{0}(response)", soapRequest.Source), xmlString);

                    xmlDoc.LoadXml(xmlString);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Logger.LogException(soapRequest.Action, ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (requestStream != null)
                {
                    requestStream.Close();
                    requestStream.Dispose();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }

            return xmlDoc;
        }

        public XmlDocument GetStaticXml(string fileName)
        {
            var xmlDoc = new XmlDocument();

            try
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                var filePath = Path.Combine(Path.GetDirectoryName(path), "StaticData", fileName);
                xmlDoc.Load(filePath);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Logger.LogException("GetStaticXml", ex);
            }

            return xmlDoc;
        }

        public void SetErrorMessage(string source, Exception ex)
        {
            Logger.LogException(source, ex);

            if (ex.Message.IndexOf("(500) Internal Server Error") > -1)
            {
                this.ErrorMessage = "(500) Internal Server Error";
            }
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();
            var properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }
            return table;
        }

        private string SafeString(string str, string defaultValue = "")
        {
            return string.IsNullOrEmpty(str) ? defaultValue : str.EscapeXMLChars();
        }

        /// <summary>
        /// C# equivalent of VB.NET Left() function.
        /// </summary>
        public string Left(string str, int length)
        {
            return str.Substring(0, length);
        }

        /// <summary>
        /// C# equivalent of VB.NET Right() function.
        /// </summary>
        public string Right(string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// C# equivalent of VB.NET Mid() function.
        /// </summary>
        public string Mid(string str, int startIndex, int length = 0)
        {
            if (length == 0)
            {
                return Right(str, str.Length - startIndex);
            }
            else
            {
                return str.Substring(startIndex - 1, length);
            }
        }
    }
}