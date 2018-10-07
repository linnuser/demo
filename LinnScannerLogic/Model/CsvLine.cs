namespace LinnScannerLogic.Model
{
    public class CsvLine
    {
        public string OrderRefernce { get; set; }
        public string Marketplace { get; set; }
        public string Name { get; set; }
        public string OrderItem​Number { get; set; }
        public string Surname { get; set; }
        public string Sku { get; set; }
        public decimal PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public string PostalService { get; set; }
        public string Postcode { get; set; }
    }
}