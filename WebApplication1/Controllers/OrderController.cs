using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class OrderController
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        [HttpGet("secured")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public IActionResult GetSecuredData()
        {
            return Ok("You have accessed secured data!");
        }
    }
}