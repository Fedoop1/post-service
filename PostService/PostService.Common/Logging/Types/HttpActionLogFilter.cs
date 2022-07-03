using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostService.Common.App.Types;
using PostService.Common.Constants;

namespace PostService.Common.Logging.Types;

public partial class HttpActionLogFilter : IAsyncActionFilter
{
    private readonly ILogger logger;
    private readonly AppOptions appOptions;
    private readonly Stopwatch watch = new ();

    public HttpActionLogFilter(ILogger<HttpActionLogFilter> logger, IOptions<AppOptions> appOptions)
    {
        this.logger = logger;
        this.appOptions = appOptions.Value;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        watch.Reset();

        var controllerName = context.Controller.GetType().Name;
        var action = context.ActionDescriptor.DisplayName;
        var arguments = string.Join('\n', context.ActionArguments.Select(pair => $"[{pair.Key}]={pair.Value}"));
        var traceId = context.HttpContext.Request.Headers[Headers.TraceHeader].ToString();

        this.LogActionExecutionStart(appOptions.Name, DateTime.Now.ToString("O"), traceId, controllerName, action, arguments);

        watch.Start();
        await next();
        watch.Stop();

        var actionTime = watch.ElapsedMilliseconds;
        this.LogActionExecutionEnd(
            appOptions.Name, 
            DateTime.Now.ToString("O"), 
            traceId, 
            controllerName, 
            action, 
            context.HttpContext.Response.StatusCode,
        TimeSpan.FromMilliseconds(actionTime).ToString("g"));
    }

    [LoggerMessage(1, LogLevel.Information,
                    "Execute controller action\n\t" +
                    "Id: {traceId}\n\t" +
                    "Controller: {controller}\n\t" +
                    "Action: {action}\n\t" +
                    "Arguments: {arguments}")]

    private partial void LogActionExecutionStart(string appName, string dateTime, string traceId, string controller, string action, string arguments);

    [LoggerMessage(2, LogLevel.Information,
        "Controller action execution end\n\t" +
        "Id: {traceId}\n\t" +
        "Controller: {controller}\n\t" +
        "Action: {action}\n\t" +
        "Status Code: {statusCode}\n\t" +
        "Time: {time}")]
    private partial void LogActionExecutionEnd(string appName, string dateTime, string traceId, string controller, string action, int statusCode,  string time);
}

