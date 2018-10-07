using System.Collections.Generic;

namespace LinnScannerLogic.Model
{
    internal class CompletedOrder
    {
        public Order Order { get; set; }

        public Shipment Shipment { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public IEnumerable<CsvLine> GenerateCsvLines()
        {
            var lines = new List<CsvLine>();
            foreach (var orderItem in OrderItems)
            {
                lines.Add(new CsvLine
                {
                    Marketplace = Order.Marketplace,
                    Name = Order.Name,
                    OrderItemNumber = orderItem.OrderItemNumber,
                    OrderRefernce = Order.OrderReference,
                    PostalService = Shipment.PostalService,
                    Postcode = Shipment.Postcode,
                    PricePerUnit = orderItem.PricePerUnit,
                    Quantity = orderItem.Quantity,
                    Sku = orderItem.Sku,
                    Surname = Order.Surname
                });
            }
            return lines;
        }
    }
}