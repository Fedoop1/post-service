using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using PostService.Common.Jwt.Services;
using PostService.Common.Jwt.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PostService.Common.Jwt.Middlewares;

namespace PostService.Common.Jwt.Extensions;

public static class JwtExtensions
{
    private const int TicksInMillisecond = 10000;
    public const string SectionName = "Jwt";

    public static void AddJwt(this WebApplicationBuilder webBuilder)
    {
        webBuilder.ConfigureJwtOptions();

        using var services = webBuilder.Services.BuildServiceProvider();

        var jwtOptions = services.GetService<IOptions<JwtOptions>>()?.Value;

        webBuilder.Services.AddSingleton<IJwtHandler, JwtHandler>();
        webBuilder.Services.AddSingleton<IAccessTokenValidator, AccessTokenValidator>();
        

        webBuilder.Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(config =>
        {
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ValidateLifetime = jwtOptions.ValidateLifetime,
                ValidateAudience = jwtOptions.ValidateAudience,
            };
        });
    }

    public static void AddAccessTokenValidation(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddTransient<AccessTokenValidationMiddleware>();

    public static void UseAccessTokenValidation(this WebApplication webApplication) =>
        webApplication.UseMiddleware<AccessTokenValidationMiddleware>();

    public static long ToTimestamp(this DateTime dateTime) =>
        (dateTime - DateTime.UnixEpoch).Ticks / TicksInMillisecond;

    public static string GetBearerToken(StringValues authorizationHeader) => authorizationHeader.SingleOrDefault()?.Split(' ')?.LastOrDefault();

    public static string GetAccessTokenCacheKey(string accessToken) => $"tokens:{accessToken}";

    private static void ConfigureJwtOptions(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<JwtOptions>().BindConfiguration(SectionName);

    
}
