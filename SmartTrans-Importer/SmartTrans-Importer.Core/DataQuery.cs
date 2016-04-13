using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartTrans_Importer.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrans_Importer.Core
{
    public class DataQuery
    {
        public static string GetRunsheet(DateTime date, string vehicle)
        {

            // every API request must contain the HTTP header 'api_key' with this value
            string apiKey = "4594135478";

            // API authenticaiton returns a token that must be included with subsequent requests as HTTP header 'X-Auth-Token'
            string authToken;

            // API authentication
            HttpWebRequest request = WebRequest.CreateHttp("https://staging.esolution.net.au/webapi/account/login");
            request.Method = "POST";
            request.ContentType = "text/json";
            request.Accept = "text/json";
            request.Headers.Add("api_key", apiKey);

            // auth vs Baycorp Proxy
            var _proxy = new WebProxy("auproxy.bcs.corp", 8080);
            _proxy.Credentials = CredentialCache.DefaultNetworkCredentials;

            request.Proxy = _proxy;

            // POST data for authentication is a JSON formatted string containing Login and Password
            byte[] content = Encoding.ASCII.GetBytes("{\"Login\":\"epadmin\",\"Password\":\"1234\"}");
            using (var stream = request.GetRequestStream())
            {
                stream.Write(content, 0, content.Length);
            }

            // Authentication request returns a JSON string with an auth-token property
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    var j = JToken.Load(new JsonTextReader(reader));
                    authToken = (string)j["auth-token"];
                }
            }

            request =
                WebRequest.CreateHttp(string.Format("https://staging.esolution.net.au/webapi/runsheet/{0:yyyy-MM-dd}/{1}",
                    date, vehicle));
            request.ContentType = "text/json";
            request.Accept = "text/json";
            request.Headers.Add("api_key", apiKey);

            // auth vs Baycorp Proxy
            request.Proxy = _proxy;

            // set header value for authentication
            request.Headers.Add("X-Auth-Token", authToken);
            response = request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
