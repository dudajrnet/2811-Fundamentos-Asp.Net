using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get([FromServices]IConfiguration configuration) 
        {
            var ambiente = configuration.GetValue<string>("Env");

            return Ok(new
            {
                Env = ambiente
            });
        }        
    }
}
