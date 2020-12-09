using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PollyStudies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class InventoryController : Controller
    {
        private static int _request = 0;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            await Task.Delay(10);
            // _request++;
            //
            // if (_request % 4 == 0)
            // {
            //     return Ok(15);
            // }

            return StatusCode((int) HttpStatusCode.InternalServerError, "something went wrong");
        }
    }
}