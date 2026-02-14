using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get() 
            => Ok();
        
    }
}
