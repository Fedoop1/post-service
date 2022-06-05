using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PostService.Common.App.Types;

namespace PostService.Operations.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    private readonly AppOptions appOptions;

    public HomeController(IOptions<AppOptions> appOptions)
    {
        this.appOptions = appOptions.Value;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        return Ok(appOptions.Name);
    }
}
