using Newtonsoft.Json;
using System.Collections.Generic;

namespace LinnScannerLogic.Model
{
    public class Order
    {
        [JsonProperty(PropertyName = "order reference")]
        public string OrderReference { get; set; }

        [JsonProperty(PropertyName = "marketplace")]
        public string Marketplace { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }
    }

    public class OrderRootObject
    {
        [JsonProperty(PropertyName = "Orders")]
        public List<Order> Data { get; set; }
    }
}