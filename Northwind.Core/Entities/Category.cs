using System.ComponentModel.DataAnnotations;

namespace Northwind.Core.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Product>? Products { get; set; }
    }
}
