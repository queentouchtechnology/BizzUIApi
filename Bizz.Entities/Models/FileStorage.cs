using  Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Bizz.Entities.Models;

public class FileStorage
{
    public int Id { get; set; }
    [Range(1, int.MaxValue, 
        ErrorMessage = "Invalid user selected")]
    public required int UserId { get; set; }
    [Required(ErrorMessage = "Missing filename")]
    public required string FileName { get; set; }
}

public class SaveFileStorageReq
{
    [Range(1, int.MaxValue, 
        ErrorMessage = "Invalid user selected")]
    [Required(ErrorMessage = "Missing user")]
    public required int UserId { get; set; }
    [Required(ErrorMessage = "Missing required information (file)")]
    public required IFormFile File { get; set; }
}

public class FileStorageRes
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? FileName { get; set; }
    public string? FilePath {get;set;}
}