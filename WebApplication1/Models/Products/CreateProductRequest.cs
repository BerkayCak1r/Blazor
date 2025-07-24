using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Products
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal? UnitPrice { get; set; }

        [Range(0, 32767)]
        public short? UnitsInStock { get; set; }

        public int? CategoryID { get; set; }
    }
}
