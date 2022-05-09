using Microsoft.AspNetCore.Mvc;
using PostService.Common.Jwt.Attributes;
using PostService.Identity.Services;

namespace PostService.Identity.Controllers;

[JwtAuth]
[Route("")]
[ApiController]
public class TokensController : ControllerBase
{
    private readonly ITokenService tokenService;

    public TokensController(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    [HttpGet("access-token/{refreshToken}/refresh")]
    public async Task<IActionResult> RefreshAccessToken(string refreshToken) => Ok(await this.tokenService.GetAccessToken(refreshToken));

    [HttpPost("refresh-token/{refreshToken}/revoke")]
    public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
    {
        await this.tokenService.RevokeRefreshToken(refreshToken);
        return NoContent();
    }
}
