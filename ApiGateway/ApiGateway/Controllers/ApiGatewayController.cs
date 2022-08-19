using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiGatewayController : ControllerBase
{
       private readonly ILogger<ApiGatewayController> _logger;

    public ApiGatewayController(ILogger<ApiGatewayController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
        yield return "ApiGateway is runing";
    }
}
