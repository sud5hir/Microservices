using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ISomeService _someService;

        private readonly IHttpClientFactory httpClientFactory;

        public TestController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var client = httpClientFactory.CreateClient("errorApi");
            var response = await client.GetAsync("api/values");
            return JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());
        }
    }
}