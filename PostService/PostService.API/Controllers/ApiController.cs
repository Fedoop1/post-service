using Microsoft.AspNetCore.Mvc;
using PostService.API.Infrastructure.Options;

namespace PostService.API.Controllers;

[Route("")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly AppOptions appOptions;

    public ApiController(AppOptions appOptions) => this.appOptions = appOptions;

    [HttpGet]
    public IActionResult Get() => Ok(appOptions.Name);
}

