using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PostService.Common.App.Types.Options;

namespace PostService.API.Controllers;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly AppOptions appOptions;

    public HomeController(IOptions<AppOptions> appOptions)
    {
        this.appOptions = appOptions.Value;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index() => Ok(appOptions.Name);
}
