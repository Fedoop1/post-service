using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PostService.Common.Tracing.Middlewares;

namespace PostService.Common.Tracing.Extensions;

public static class TracingExtensions
{
    public static void AddTracing(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddScoped<TraceRequestModifierMiddleware>();

    public static void UseTracing(this WebApplication webApplication) =>
        webApplication.UseMiddleware<TraceRequestModifierMiddleware>();
}
