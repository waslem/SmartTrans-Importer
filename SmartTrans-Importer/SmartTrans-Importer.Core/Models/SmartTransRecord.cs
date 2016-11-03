using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrans_Importer.Core.Models
{

    public class SmartTransRecord
    {
        [JsonProperty("OrderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty("DeliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [JsonProperty("ETA")]
        public string ETA { get; set; }

        [JsonProperty("SiteTime")]
        public string SiteTime { get; set; }

        [JsonProperty("Arrival")]
        public string Arrival { get; set; }

        [JsonProperty("Departure")]
        public string Departure { get; set; }

        [JsonProperty("SiteTimeActual")]
        public string SiteTimeActual { get; set; }

        [JsonProperty("Customer")]
        public string Customer { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Suburb")]
        public string Suburb { get; set; }

        [JsonProperty("Postcode")]
        public int? Postcode { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Outcome")]
        public string Reasons { get; set; }

        [JsonProperty("CustomerRef")]
        public string CustomerRef { get; set; }

        [JsonProperty("TW_start")]
        public string TW_start { get; set; }

        [JsonProperty("TW_end")]
        public string TW_end { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Comments")]
        public string Comments { get; set; }

        [JsonProperty("DriverNote")]
        public string DriverNote { get; set; }

        [JsonProperty("DriverComment")]
        public string DriverComment { get; set; }

        [JsonProperty("Vehicle")]
        public string Vehicle { get; set; }

        [JsonProperty("X")]
        public double? X { get; set; }

        [JsonProperty("Y")]
        public double? Y { get; set; }

        [JsonProperty("GpsX")]
        public double? GpsX { get; set; }

        [JsonProperty("GpsY")]
        public double? GpsY { get; set; }

        [JsonProperty("Driver")]
        public string Driver { get; set; }

    }
}
