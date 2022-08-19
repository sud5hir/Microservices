using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
   [HttpGet]  
    public IEnumerable<string> Get()  
    {  
        return new string[] { "Surface Book 2", "Mac Book Pro" };  
    }  
}
