using Microsoft.AspNetCore.Mvc;
using Bizz.Entities.Models;
using Bizz.DataService.Services;

namespace BizzUIApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileStorageController(FileStorageService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Response>> CreateFile([FromForm] SaveFileStorageReq req)
    {
        var result = await service.Create(req);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ListResponse<FileStorageRes>>> List()
    {
        var result = await service.GetList();
        return Ok(result);
    }
}