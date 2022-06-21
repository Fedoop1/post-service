using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PostService.Common.LoadBalancing.Exceptions;
using PostService.Common.LoadBalancing.Types;
using RestEase;

namespace PostService.Common.LoadBalancing.Extensions;

public static class LoadBalancingExtensions
{
    public const string SectionName = "LoadBalancing";

    public static void AddLoadBalancing(this WebApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddOptions<ServiceRegisterOptions>().BindConfiguration(SectionName);
        webBuilder.Services.AddSingleton<IServiceRegister, DefaultServiceRegister>();
    }

    public static void RegisterServiceForwarder<T>(this WebApplicationBuilder webBuilder, string serviceName) where T : class
    {
        using var services = webBuilder.Services.BuildServiceProvider();

        var serviceRegister = services.GetService<IServiceRegister>();
        serviceRegister?.RegisterHttpClient(webBuilder, serviceName);

        webBuilder.Services.AddTransient(provider =>
            new RestClient(provider.GetService<IHttpClientFactory>()?.CreateClient(serviceName) ??
                           throw new HttpClientException($"HttpClient with {serviceName} name doesn't exist"))
                .For<T>());
    }
}
