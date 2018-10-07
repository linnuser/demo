using Newtonsoft.Json;
using System.Collections.Generic;

namespace LinnScannerLogic.Model
{
    public class OrderItem
    {
        [JsonProperty(PropertyName = "order reference")]
        public string OrderReference { get; set; }

        [JsonProperty(PropertyName = "marketplace")]
        public string Marketplace { get; set; }

        [JsonProperty(PropertyName = "order item number")]
        public string OrderItemNumber { get; set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "price per unit")]
        public decimal PricePerUnit { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }
    }

    public class OrderItemRootObject
    {
        [JsonProperty(PropertyName = "order items")]
        public List<OrderItem> Data { get; set; }
    }
}