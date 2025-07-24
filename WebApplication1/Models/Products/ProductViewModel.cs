namespace WebApplication1.Models.Products
{
    public class ProductViewModel
    {
        public string ProductName { get; set; } = string.Empty;

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }
    }
}
