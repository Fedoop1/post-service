using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PostService.Identity.Infrastructure.Options;
using PostService.Identity.Models.JWT.Interfaces;
using PostService.Identity.Services;
using PostService.Identity.Services.Interfaces;

namespace PostService.Identity.Infrastructure.Extensions;
public static class JwtExtensions
{
    public static void AddJwt(this WebApplicationBuilder webBuilder)
    {
        webBuilder.ConfigureJwtOptions();

        using var services = webBuilder.Services.BuildServiceProvider();

        var jwtOptions = services.GetService<JwtOptions>();

        webBuilder.Services.AddSingleton<IJwtHandler, JwtHandler>();
        webBuilder.Services.AddScoped<IIdentityService, IdentityService>();

        webBuilder.Services.AddAuthentication().AddJwtBearer(config =>
        {
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtOptions.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),
                RequireExpirationTime = true,
            };
        });
    }
}
