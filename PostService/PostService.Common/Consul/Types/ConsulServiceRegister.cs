using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostService.Common.LoadBalancing.Types;

namespace PostService.Common.Consul.Types;

public class ConsulServiceRegister : IServiceRegister
{
    public void RegisterHttpClient(WebApplicationBuilder webBuilder, string serviceName)
    {
        webBuilder.Services.AddHttpClient(serviceName).AddHttpMessageHandler(provider =>
            new ConsulDiscoveryHandler(
                provider.GetService<IConsulServiceRegistry>(),
                provider.GetService<IOptions<ConsulOptions>>(), 
                serviceName));
    }
}
