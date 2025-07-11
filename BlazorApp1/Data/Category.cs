using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Data
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}
