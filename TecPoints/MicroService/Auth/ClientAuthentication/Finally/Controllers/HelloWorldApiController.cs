using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClientAuthentication.Controllers
{
    [ApiController]
    public class HelloWorldApiController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public HelloWorldApiController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        [Route("[controller]/[action]")]
        public IActionResult SayHelloPublic()
        {
            return Ok("Hello world for everyone");
        }

        // //sayhello secured
        [HttpGet()]
        [Authorize()]
        [Route("[controller]/[action]")]
        public IActionResult SayHelloSecured()
        {
            return Ok("you are authrozied");
        }

                // //sayhello secured
        [HttpGet()]
        [Authorize(Policy = "isclient")]
        [Route("[controller]/[action]")]
        public IActionResult SayHelloSecuredByClientIDCheck()
        {
            return Ok("you are expected client");
        }

        [HttpGet()]
        [Authorize(Policy = "apiscope")]
        [Route("[controller]/[action]")]
        public IActionResult SayHelloSecuredByApiScopeCheck()
        {
            return Ok("you have permission on this api");
        }

        [HttpGet()]
        [Authorize(Policy = "custom")]
        [Route("[controller]/[action]")]
        public IActionResult SayHelloSecuredByCustomFuncCheck()
        {
            return Ok("you pass permission check");
        }
    }
}
