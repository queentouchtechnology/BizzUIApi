using Microsoft.AspNetCore.Mvc;

using Bizz.Entities.Models;
using Bizz.DataService.Services;

namespace BizzUIApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ProductService service) : ControllerBase
{
    private static List<Product> products = new List<Product>();

    private static int productIdCounter = 1;
    private int GenerateUniqueId()
    {
        return productIdCounter++;
    }


    [HttpPost]
        public ActionResult<Response> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data");
            }

            // Assuming you have some logic to assign a unique ID to the product
            product.Id = GenerateUniqueId();
            products.Add(product);

           var result = service.Create(product);

        return Ok(result);

        // return Ok(new ApiResponse<List<Product>>
        // {
        //     Status = true,
        //     RespText = "Product created successfully",
        //     Data = products
        // });
    }

        
    //
    // [HttpPost("test")]
    // public ActionResult<ListResponse<Product>> Test()
    // {
    //     var insertResult = service.InsertDummyData();
    //     Console.WriteLine($"Insert Result: {0}",insertResult);
    //     var result = service.GetProducts();
    //     return Ok(result);
    // }
    //
    // [HttpPost("insertdummydata")]
    // public ActionResult<Response> InsertDummyData()
    // {
    //     var result = service.InsertDummyData();
    //     return Ok(result);
    // }

    [HttpGet("get/{id}")]
    public ActionResult Get(int id)
    {

        int productId = id;
        var product = service.GetProductById(productId);

        if (product == null)
        {
            return NotFound(); // Returns 404 Not Found if the product with the given id is not found.
        }

        return Ok(product); // Returns 200 OK with the product data if found.

       
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        var isDeleted = service.DeleteProductById(id);

        if (isDeleted)
        {
            return NoContent(); // Successfully deleted
        }
        else
        {
            return NotFound(); // Product with the specified id was not found or deletion failed.
        }
    }

    [HttpGet]
    public ActionResult<ListResponse<Product>> List()
    {
        var result = service.GetProducts();
        return Ok(result);
    }
}