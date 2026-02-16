using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [ApiKey]
        [HttpGet("")]
        public IActionResult Get() 
            => Ok();        
    }
}
