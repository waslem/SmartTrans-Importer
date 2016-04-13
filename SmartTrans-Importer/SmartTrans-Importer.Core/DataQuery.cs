using Newtonsoft.Json;
using SmartTrans_Importer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrans_Importer.Core
{
    public class DataQuery
    {

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
