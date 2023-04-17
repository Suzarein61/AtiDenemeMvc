namespace WebApplication4.Models
{
    public class DenemeModel
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public int CategoryID { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }
    }
}
