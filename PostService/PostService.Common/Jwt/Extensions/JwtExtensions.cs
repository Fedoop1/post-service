using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PostService.Common.Jwt.Services;
using PostService.Common.Jwt.Types;

namespace PostService.Common.Jwt.Extensions;
public static class JwtExtensions
{
    public static void ConfigureJwtOptions(this WebApplicationBuilder webBuilder)
    {
        using var serviceProvider = webBuilder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        webBuilder.Services.Configure<JwtOptions>(configuration?.GetSection("Jwt"));
    }

    public static void AddJwt(this WebApplicationBuilder webBuilder)
    {
        webBuilder.ConfigureJwtOptions();

        using var services = webBuilder.Services.BuildServiceProvider();

        var jwtOptions = services.GetService<JwtOptions>();

        webBuilder.Services.AddSingleton<IJwtHandler, JwtHandler>();

        webBuilder.Services.AddAuthentication().AddJwtBearer(config =>
        {
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtOptions.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),
                ValidateLifetime = jwtOptions.ValidateLifetime,
            };
        });
    }
}
