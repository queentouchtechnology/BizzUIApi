namespace BizzUIBlazor.Components.Models
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string RespText { get; set; }
        public T Data { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
