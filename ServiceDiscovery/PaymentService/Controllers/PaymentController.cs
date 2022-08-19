using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()  
    {  
        return new string[] { "Catcher Wong", "James Li" };  
    }  
  
    [HttpGet("{id}")]  
    public string Get(int id)  
    {  
        return $"Catcher Wong - {id}";  
    }         
    
}
