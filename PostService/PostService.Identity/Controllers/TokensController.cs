using Microsoft.AspNetCore.Mvc;
using PostService.Common.Jwt.Attributes;
using PostService.Identity.Services;
using PostService.Common.Jwt.Extensions;

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

    [HttpGet("me")]
    public IActionResult GetTokenPayload() =>
        Ok(this.tokenService.GetTokenPayload(JwtExtensions.GetBearerToken(this.Request.Headers.Authorization)));

    [HttpGet("access-token/{refreshToken}/refresh")]
    public async Task<IActionResult> RefreshAccessToken(string refreshToken) => Ok(await this.tokenService.GetAccessTokenAsync(refreshToken));

    [HttpGet("access-token/{accessToken}/revoke")]
    public async Task<IActionResult> RevokeAccessToken(string accessToken)
    {
        await this.tokenService.RevokeAccessTokenAsync(accessToken);
        return NoContent();
    }

    [HttpPost("refresh-token/{refreshToken}/revoke")]
    public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
    {
        await this.tokenService.RevokeRefreshTokenAsync(refreshToken);
        return NoContent();
    }
}
