
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]  
        public IActionResult Register()
        {
            return Ok("Register endpoint hit");
        }
        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok("Login endpoint hit");
        }
    }
}
