namespace BlazorApp1.ViewModels
{

    public class Rootobject
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public int Discontinued { get; set; }
        public string QuantityPerUnit { get; set; }
        public float UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
    }

}
