using PostService.Identity.Infrastructure.Options;


namespace PostService.Identity.Infrastructure.Extensions;
public static class StartupExtensions
{
    public static void ConfigureAppOptions(this WebApplicationBuilder webBuilder)
    {
        using var serviceProvider = webBuilder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        webBuilder.Services.Configure<AppOptions>(configuration?.GetSection("App"));
    }

    public static void ConfigureJwtOptions(this WebApplicationBuilder webBuilder)
    {
        using var serviceProvider = webBuilder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        webBuilder.Services.Configure<JwtOptions>(configuration?.GetSection("Jwt"));
    }
}
