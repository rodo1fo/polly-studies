using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PollyStudies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CatalogController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000/")};

            var request = $"inventory/{id}";

            var response = await httpClient.GetAsync(request);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int) response.StatusCode, response.Content.ReadAsStringAsync());
            
            var itemsInStock = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            return Ok(itemsInStock);
        }
    }
}