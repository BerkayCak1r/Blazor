using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Core.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public int? CategoryID { get; set; }

        public Category? Category { get; set; }

        public string? ImageUrl { get; set; }

    }
}
