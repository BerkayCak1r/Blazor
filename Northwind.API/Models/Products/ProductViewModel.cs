namespace Northwind.API.Models.Products
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public int? CategoryID { get; set; }

        public string? CategoryName { get; set; } 

        public string? ImageUrl { get; set; }

    }
}
