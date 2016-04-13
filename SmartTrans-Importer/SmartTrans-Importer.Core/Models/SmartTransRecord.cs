using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrans_Importer.Core.Models
{
    public class SmartTransRun
    {

        public string Status;

        public string Agent { get; set; }
        public DateTime RunDate { get; set; }
        public List<SmartTransRecord> RecordList { get; set; }
    }

    public class SmartTransRecord
    {
        public string Order { get; set; }
        public string DeliveryDate { get; set; }
        public string ETA { get; set; }
        public double SiteTime { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
        public int? SiteTimeActual { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string Reasons { get; set; }
        public string CustomerRef { get; set; }
        public string TW_start { get; set; }
        public string TW_end { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public object DriverNote { get; set; }
        public string DriverComment { get; set; }
        public string Vehicle { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double? GpsX { get; set; }
        public double? GpsY { get; set; }
        public string Driver { get; set; }

    }
}
