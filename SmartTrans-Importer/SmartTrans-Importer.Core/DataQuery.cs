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

            request.Credentials = CredentialCache.DefaultCredentials;

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

        public SmartTransRun GetRunsheetData(String Agent, DateTime RunDate)
        {
            try
            {
                var client = new WebClient();
                //client.Headers.Add("User-Agent", "Nobody"); //my endpoint needs this...
                // get token and verify etc here

                var username = Properties.Resources.SmartTrans_Username;
                var password = Properties.Resources.SmartTrans_Password;

                var url = string.Format(Properties.Resources.ApiUrl, Agent, RunDate.ToString("dd/MM/yyyy"));

                var response = client.DownloadString(new Uri(url));

                SmartTransRun result = JsonConvert.DeserializeObject<SmartTransRun>(response);

                result.Status = "Success";

                return result;

            }
            catch (Exception ex)
            {
                SmartTransRun result = new SmartTransRun();

                result.Status = "Error - " + ex.InnerException.Message;

                return result;
            }
        }
    }
}
