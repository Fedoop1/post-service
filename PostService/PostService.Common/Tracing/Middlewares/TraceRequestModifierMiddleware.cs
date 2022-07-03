using Microsoft.AspNetCore.Http;
using PostService.Common.Constants;

namespace PostService.Common.Tracing.Middlewares;

public class TraceRequestModifierMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (string.IsNullOrEmpty(context.Request.Headers[Headers.TraceHeader]))
        {
            context.Request.Headers[Headers.TraceHeader] = Guid.NewGuid().ToString("N");
        }

        await next(context);

        if (!string.IsNullOrEmpty(context.Request.Headers[Headers.TraceHeader]))
        {
            context.Request.Headers[Headers.TraceHeader] = string.Empty;
        }
    }
}

