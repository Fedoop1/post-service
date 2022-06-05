using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PostService.Common.App.Types;
using PostService.Common.Enums;
using PostService.Common.Types;

namespace PostService.Common.App.Extensions;

public static class AppExtensions
{
    public const string SectionName = "App";

    public static void ConfigureAppOptions(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<AppOptions>().BindConfiguration(SectionName);

    public static void RegisterProviders(this WebApplicationBuilder webBuilder)
    {
        var assembly = Assembly.GetEntryAssembly();
        var assemblyTypes = assembly.DefinedTypes;

        var assemblyInjectableInterfaces = assemblyTypes.Where(assemblyType =>
            assemblyType.IsInterface && assemblyType.GetCustomAttribute<InjectableAttribute>() is not null);

        if (!assemblyInjectableInterfaces.Any()) return;

        var assemblyClasses = assemblyTypes.Where(assemblyType => assemblyType.IsClass);

        foreach (var assemblyClass in assemblyClasses)
        {
            foreach (var assemblyInterface in assemblyInjectableInterfaces)
            {
                if (assemblyClass.IsAssignableTo(assemblyInterface))
                {
                    var injectableAttribute = assemblyInterface.GetCustomAttribute<InjectableAttribute>();
                    switch (injectableAttribute?.Scope)
                    {
                        case Scope.Transient:
                            webBuilder.Services.TryAddTransient(assemblyInterface, assemblyClass);
                            break;
                        case Scope.Scoped:
                            webBuilder.Services.TryAddTransient(assemblyInterface, assemblyClass);
                            break;
                        case Scope.Singleton:
                            webBuilder.Services.TryAddTransient(assemblyInterface, assemblyClass);
                            break;
                    }
                }
            }
        }
    }
}

