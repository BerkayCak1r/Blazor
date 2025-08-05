namespace BlazorApp1.ViewModels
{
	public class OrderViewModel
	{
		public int OrderID { get; set; }
		public string CustomerID { get; set; }
		public string CustomerName { get; set; }
		public int? EmployeeID { get; set; }
		public string EmployeeName { get; set; }
		public DateTime? OrderDate { get; set; }
		public decimal? Freight { get; set; }
		public string ShipCity { get; set; }
		public string ShipCountry { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
