using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace PollyStudies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CatalogController : Controller
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> _httpRetryPolicy;
        
        public CatalogController()
        {
            _httpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) / 2));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000/")};

            var request = $"inventory/{id}";

            var response = await _httpRetryPolicy.ExecuteAsync(() => httpClient.GetAsync(request));

            if (!response.IsSuccessStatusCode)
                return StatusCode((int) response.StatusCode, response.Content.ReadAsStringAsync());
            
            var itemsInStock = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            return Ok(itemsInStock);
        }
    }
}