namespace WebApplication1.Models.Orders
{
    public class OrderDetailItem
    {
        public int ProductID { get; set; }           
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }       
        public short Quantity { get; set; }           
        public float Discount { get; set; }           
    }

    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; }
        public decimal? Freight { get; set; }
        public string ShipCity { get; set; } = string.Empty;
        public string ShipCountry { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderDetailItem> OrderDetails { get; set; } = new();
    }
}
