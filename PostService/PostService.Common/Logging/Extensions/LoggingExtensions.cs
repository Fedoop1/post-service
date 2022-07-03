using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using PostService.Common.Logging.Types;

namespace PostService.Common.Logging.Extensions;

public static class LoggingExtensions
{
    public static void AddControllerLogging(this WebApplicationBuilder webBuilder) => 
        webBuilder.Services.AddControllers(config =>
    {
        config.Filters.Add<HttpActionLogFilter>();
        config.Filters.Add<HttpActionErrorLogFilter>();
    });

    public static void AddConsoleLoggingFormatter(this WebApplicationBuilder webBuilder) => 
        webBuilder.Services.AddLogging(config =>
    {
        config.AddConsole().AddConsoleFormatter<AppConsoleFormatter, ConsoleWrappingFormatterOptions>();
    });
}
