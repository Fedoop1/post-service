using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PostService.Common.Types;

namespace PostService.Common.Extensions;
public static class StartupExtensions
{
    public static void AddStartupInitializer(this WebApplicationBuilder webBuilder, params Type[] initializers)
    {
        if (!initializers.Any()) return;

        webBuilder.Services.AddTransient<IStartupInitializer, StartupInitializer>(provider =>
        {
            var startupInitializer = new StartupInitializer();
            foreach (var initializer in initializers.Where(initializer => typeof(IInitializer).IsAssignableFrom(initializer)))
            {
                var initializationProvider = provider.GetService(initializer) as IInitializer;
                startupInitializer.AddInitializer(initializationProvider);
            }

            return startupInitializer;
        });

    }


    public static async void InitializeAsync(this WebApplication webApplication)
    {
        var startupInitializer = webApplication.Services.GetService<IStartupInitializer>();
        await startupInitializer.InitializeAsync();
    }
}
