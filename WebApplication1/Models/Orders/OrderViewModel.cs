namespace WebApplication1.Models.Orders
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public string? CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipCountry { get; set; }
        public string? EmployeeName { get; set; }
    }
}
