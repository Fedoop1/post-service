using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using PostService.Common.Jwt.Extensions;
using PostService.Common.Jwt.Types;

namespace PostService.Common.Jwt.Middlewares;
public class AccessTokenValidationMiddleware : IMiddleware
{
    private readonly IDistributedCache distributedCache;
    private readonly IAccessTokenValidator accessTokenValidator;

    public AccessTokenValidationMiddleware(IDistributedCache distributedCache, IAccessTokenValidator accessTokenValidator)
    {
        this.distributedCache = distributedCache;
        this.accessTokenValidator = accessTokenValidator;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authorizationHeader = context.Request.Headers.Authorization.ToString();

        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            var accessToken = JwtExtensions.GetBearerToken(authorizationHeader);

            if (!this.accessTokenValidator.ValidateToken(accessToken).isValid || 
                string.IsNullOrEmpty(await this.distributedCache.GetStringAsync(JwtExtensions.GetAccessTokenCacheKey(accessToken))))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await next(context);
    }
}
