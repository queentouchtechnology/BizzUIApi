namespace Bizz.Entities.Models;

public class CommonResponse
{
    public bool Status { get; set; } = true;
    public string RespText { get; set; } = "ok";
}
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

public class SaveProduct
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}