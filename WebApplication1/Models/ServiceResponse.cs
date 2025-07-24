namespace WebApplication1.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
         public bool Success => string.IsNullOrEmpty(Error);
        public string Error { get; set; }
    }
}
