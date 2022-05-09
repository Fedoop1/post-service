using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PostService.Common.Jwt.Services;
using PostService.Common.Jwt.Types;

namespace PostService.Common.Jwt.Extensions;
public static class JwtExtensions
{
    public const string SectionName = "Jwt";

    public static void AddJwt(this WebApplicationBuilder webBuilder)
    {
        webBuilder.ConfigureJwtOptions();

        using var services = webBuilder.Services.BuildServiceProvider();

        var jwtOptions = services.GetService<IOptions<JwtOptions>>()?.Value;

        webBuilder.Services.AddSingleton<IJwtHandler, JwtHandler>();

        webBuilder.Services.AddAuthentication().AddJwtBearer(config =>
        {
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtOptions.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ValidateLifetime = jwtOptions.ValidateLifetime,
            };
        });
    }

    private static void ConfigureJwtOptions(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<JwtOptions>().BindConfiguration(SectionName);
}
