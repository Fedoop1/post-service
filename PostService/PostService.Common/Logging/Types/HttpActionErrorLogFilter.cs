using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostService.Common.App.Types;
using PostService.Common.Constants;

namespace PostService.Common.Logging.Types;

public partial class HttpActionErrorLogFilter : IAsyncExceptionFilter
{
    private readonly ILogger logger;
    private readonly AppOptions appOptions;

    public HttpActionErrorLogFilter(ILogger<HttpActionErrorLogFilter> logger, IOptions<AppOptions> appOptions)
    {
        this.logger = logger;
        this.appOptions = appOptions.Value;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var traceId = context.HttpContext.Request.Headers[Headers.TraceHeader];
        var errorName = context.Exception.GetType().Name;

        this.LogError(this.appOptions.Name, DateTime.Now.ToString("O"), traceId, errorName, context.Exception.Message);

        return Task.CompletedTask;
    }

    [LoggerMessage(1, LogLevel.Error, 
                                      "An error occurred\n\t" +
                                      "Id: {traceId}\n\t" +
                                      "Error: {error}\n\t" +
                                      "Details: {details}")]
    private partial void LogError(string appName, string dateTime, string traceId, string error, string details);
}
