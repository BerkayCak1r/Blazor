using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Data
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Ürün adı boş bırakılamaz")]
        [StringLength(50, ErrorMessage = "Ürün adı en fazla 50 karakter olabilir")]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Fiyat negatif olamaz")]
        public decimal? UnitPrice { get; set; }

        [Range(0, 32767, ErrorMessage = "Stok negatif olamaz")]
        public short? UnitsInStock { get; set; }

        public int? CategoryID { get; set; }
        public Category? Category { get; set; }

        public string? ImageUrl { get; set; }
    }
}
