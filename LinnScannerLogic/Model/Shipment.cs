using Newtonsoft.Json;
using System.Collections.Generic;

namespace LinnScannerLogic.Model
{
    public class Shipment
    {
        [JsonProperty(PropertyName = "order reference")]
        public string OrderRefernce { get; set; }

        [JsonProperty(PropertyName = "marketplace")]
        public string Marketplace { get; set; }

        [JsonProperty(PropertyName = "postal service")]
        public string PostalService { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }
    }

    public class ShipmentRootObject
    {
        [JsonProperty(PropertyName = "Shipments")]
        public List<Shipment> Data { get; set; }
    }
}