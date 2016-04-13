using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrans_Importer.Core
{
    public class DriverDB
    {
        public Dictionary<String, String> DriverDictionary { get; set; }

        public DriverDB()
        {
            DriverDictionary = new Dictionary<string, string>();
            // load the drivers from disk
        }

        public bool Add(String DriverName, String CollectInitials)
        {

            return true;
        }

        public bool Remove(String DriverName, String CollectInitials)
        {

            return true;
        }

        public bool SaveToFile()
        {
            return true;
        }

        public string GetDriver(String CollectInitials)
        {
            return "";
        }

        public string GetCollectInitials(String DriverName)
        {
            return "";
        }
    }

}
