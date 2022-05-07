using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostService.Common.App.Types.Options;

namespace PostService.Common.App.Extensions;

public static class AppExtensions
{
    public static void ConfigureAppOptions(this WebApplicationBuilder webBuilder)
    {
        using var serviceProvider = webBuilder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        webBuilder.Services.Configure<AppOptions>(configuration?.GetSection("App"));
    }
}

