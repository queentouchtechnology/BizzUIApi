namespace Bizz.Entities.Models;

public class Response
{
    public bool Status { get; set; } = true;
    public string RespText { get; set; } = "ok";
}

public class GetResponse<T>
{
    public bool Status { get; set; } = true;
    public string RespText { get; set; } = "ok";
    public T? Data { get; set; }
}


public class ListResponse<T>
{
    public bool Status { get; set; } = true;
    public string RespText { get; set; } = "ok";
    public int Count { get; set; }
    public List<T> Data { get; set; } = [];
}