using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostService.Common.LoadBalancing.Exceptions;

namespace PostService.Common.LoadBalancing.Types;

public class DefaultServiceRegister : IServiceRegister
{
    private readonly ServiceRegisterOptions serviceRegisterOptions;

    public DefaultServiceRegister(IOptions<ServiceRegisterOptions> serviceRegisterOptions)
    {
        this.serviceRegisterOptions = serviceRegisterOptions.Value;
    }

    public void RegisterHttpClient(WebApplicationBuilder webBuilder, string serviceName)
    {
        var service = this.serviceRegisterOptions.Services.FirstOrDefault(s =>
                          s.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase)) ??
                      throw new ServiceProviderException(
                          $"$There is no configuration for service with {serviceName} name");

        webBuilder.Services.AddHttpClient(serviceName, client =>
        {
            var baseAddress = new UriBuilder()
            {
                Scheme = service.Scheme,
                Host = service.Host,
                Port = service.Port,

            }.Uri;

            client.BaseAddress = baseAddress;
        });
    }
}
