using System.ComponentModel.DataAnnotations;

namespace Northwind.Core.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // 1e N ilişkisi
        public ICollection<Order>? Orders { get; set; }
    }
}
