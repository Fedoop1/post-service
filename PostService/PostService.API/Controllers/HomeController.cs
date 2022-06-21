﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PostService.Common.App.Types;

namespace PostService.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("")]
public class HomeController : ControllerBase
{
    private readonly AppOptions appOptions;

    public HomeController(IOptions<AppOptions> appOptions)
    {
        this.appOptions = appOptions.Value;
    }

    [HttpGet("")]
    public IActionResult Index() => Ok(appOptions.Name);

    [HttpGet("ping")]
    public IActionResult Ping() => Ok("pong");
}
