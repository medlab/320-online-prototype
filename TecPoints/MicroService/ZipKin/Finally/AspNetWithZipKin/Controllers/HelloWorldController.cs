using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWithZipKin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var httpClient = new HttpClient();
            var result=await httpClient.GetStringAsync("https://github.com");
            return Ok($"hello from github.com: {result.Substring(0,255)}");
        }
    }
}