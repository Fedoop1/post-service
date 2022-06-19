using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PostService.Common.Consul.Types;

namespace PostService.Common.Consul.Extensions;
public static class ConsulExtensions
{
    public const string SectionName = "Consul";

    public static void AddConsul(this WebApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddOptions<ConsulOptions>().BindConfiguration(SectionName);

        webBuilder.Services.AddSingleton<IConsulServiceRegistry, ConsulServiceRegistry>();
        webBuilder.Services.AddSingleton<IConsulClient>(p => new ConsulClient(config =>
        {
            var consulOptions = p.GetService<IOptions<ConsulOptions>>().Value;
            if (!string.IsNullOrEmpty(consulOptions.ClusterAddress))
            {
                config.Address = new Uri(consulOptions.ClusterAddress);
            }

        }));
    }

    public static void UseConsul(this IHost host)
    {
        var consulOptions = host.Services.GetService<IOptions<ConsulOptions>>()?.Value;
        var serviceId = Guid.NewGuid().ToString("N");
        var consulClient = host.Services.GetService<IConsulClient>();
        var scheme =
            !string.IsNullOrEmpty(consulOptions?.ServiceAddress) && consulOptions.ServiceAddress.StartsWith("http")
                ? string.Empty
                : "http://";

        consulClient?.Agent.ServiceRegister(new AgentServiceRegistration()
        {
            Address = consulOptions.ServiceAddress,
            Port = consulOptions.ServicePort,
            ID = serviceId,
            Name = consulOptions.ServiceName,
            Check = consulOptions.PingEnabled
                ? new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(consulOptions.PingInterval <= 0 ? 10 : consulOptions.PingInterval),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulOptions.RemoveAfterInterval <= 0
                        ? 10
                        : consulOptions.RemoveAfterInterval),
                    HTTP =
                        $"{scheme}{consulOptions.ClusterAddress}:{consulOptions.ServicePort}/{consulOptions.PingEndpoint}"

                }
                : null,

        });

        host.Services.GetService<IHostApplicationLifetime>()!.ApplicationStopping.Register(async () =>
        {
            await consulClient?.Agent.ServiceDeregister(serviceId);
        });
    }
}
