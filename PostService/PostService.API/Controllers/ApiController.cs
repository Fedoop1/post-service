using Microsoft.AspNetCore.Mvc;

namespace PostService.API.Controllers;

[Route("")]
[ApiController]
public class ApiController : ControllerBase
{
    [Route("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}

