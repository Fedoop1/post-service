using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostService.Common.CORS.Types;

namespace PostService.Common.CORS.Extensions;

public static class CorsExtensions
{
    public const string SectionName = "CORS";

    public static void AddCors(this WebApplicationBuilder webBuilder)
    {
        ConfigureCorsOptions(webBuilder);

        using var serviceProvider = webBuilder.Services.BuildServiceProvider();
        var corsOptions = serviceProvider.GetService<IOptions<CorsOptions>>()?.Value;

        webBuilder.Services.AddCors((options) =>
        {
            options.AddPolicy("Default", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                builder.SetIsOriginAllowed(_ => true);
            });

            if (corsOptions?.Policies is null || corsOptions.Policies.Any()) return;

            foreach (var policy in corsOptions.Policies)
            {
                options.AddPolicy(policy.Name, config =>
                {
                    if (policy.AllowAnyHeaders.HasValue && policy.AllowAnyHeaders.Value)
                    {
                        config.AllowAnyHeader();
                    }
                    else
                    {
                        config.WithHeaders(policy.AllowedHeaders ?? Array.Empty<string>());
                    }

                    if (policy.AllowAnyMethods.HasValue && policy.AllowAnyMethods.Value)
                    {
                        config.AllowAnyMethod();
                    }
                    else
                    {
                        config.WithMethods(policy.AllowedMethods ?? Array.Empty<string>());
                    }

                    if (policy.AllowAnyOrigins.HasValue && policy.AllowAnyOrigins.Value)
                    {
                        config.SetIsOriginAllowed(_ => true);
                    }
                    else
                    {
                        config.WithOrigins(policy.AllowedOrigins ?? Array.Empty<string>());
                    }

                    if (policy.AllowCredentials)
                    {
                        config.AllowCredentials();
                    }
                });
            }
        });
    }

    public static void UseCors(this WebApplication webApplication)
    {
        var corsOptions = webApplication.Services.GetService<IOptions<CorsOptions>>()?.Value;

        webApplication.UseCors(corsOptions.Policy ?? "Default");
    }

    private static void ConfigureCorsOptions(WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<CorsOptions>().BindConfiguration(SectionName);
}
