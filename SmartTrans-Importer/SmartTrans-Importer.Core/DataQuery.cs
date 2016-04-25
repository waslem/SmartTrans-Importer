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
            HttpWebRequest request = WebRequest.CreateHttp(Settings.Default.ApiUrl + "/webapi/account/login");
            request.Method = "POST";
            request.ContentType = "text/json";
            request.Accept = "text/json";
            request.Headers.Add("api_key", apiKey);
  
            int port;
            WebProxy _proxy;

            bool isValid = Int32.TryParse(Settings.Default.ProxyPort, out port);
            // auth vs Baycorp Proxy
            // check if setting is an int before using proxy - not sure if this is best?
            if (isValid)
            {
                _proxy = new WebProxy(Settings.Default.ProxyAddress, port);
                _proxy.Credentials = CredentialCache.DefaultNetworkCredentials;

                request.Proxy = _proxy;
            }

            // POST data for authentication is a JSON formatted string containing Login and Password
            // Create JSON formatted string using json.net and settings from program options
            string user = Settings.Default.eSolutionsLogin;
            string pass = Settings.Default.eSolutionsPassword;

            JObject o = new JObject();

            o.Add("Login", user);
            o.Add("Password", pass);

            byte[] content = Encoding.ASCII.GetBytes(o.ToString());
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

            request = WebRequest.CreateHttp(string.Format(Settings.Default.ApiUrl + "/webapi/runsheet/{0:yyyy-MM-dd}/{1}", date, vehicle));

            request.ContentType = "text/json";
            request.Accept = "text/json";
            request.Headers.Add("api_key", apiKey);

            isValid = Int32.TryParse(Settings.Default.ProxyPort, out port);

            // auth vs Baycorp Proxy
            // check if setting is an int before using proxy - not sure if this is best?
            if (isValid)
            {
                _proxy = new WebProxy(Settings.Default.ProxyAddress, port);
                _proxy.Credentials = CredentialCache.DefaultNetworkCredentials;

                request.Proxy = _proxy;
            }

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
