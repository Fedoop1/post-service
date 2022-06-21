using Microsoft.Extensions.Options;
using Polly;
using PostService.Common.Consul.Exceptions;

namespace PostService.Common.Consul.Types;

public class ConsulDiscoveryHandler : DelegatingHandler
{
    private readonly IConsulServiceRegistry serviceRegistry;
    private readonly ConsulOptions consulOptions;
    private readonly string serviceName;

    public ConsulDiscoveryHandler(IConsulServiceRegistry serviceRegistry, IOptions<ConsulOptions> consulOptions, string serviceName)
    {
        this.serviceRegistry = serviceRegistry;
        this.consulOptions = consulOptions.Value;
        this.serviceName = serviceName;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uri = new Uri(
            $"{request.RequestUri.Scheme}://{serviceName}/{request.RequestUri.Host}{request.RequestUri.PathAndQuery}");

        return await SendAsync(request, cancellationToken, uri);
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, Uri uri) =>
        await Policy.Handle<Exception>()
            .WaitAndRetryAsync(this.consulOptions.RequestRetries <= 0 ? 0 : this.consulOptions.RequestRetries,
                retry => this.consulOptions.RetriesInterval).ExecuteAsync(async () =>
            {
                request.RequestUri = await GetUriAsync(uri);

                return await base.SendAsync(request, cancellationToken);
            });

    private async Task<Uri> GetUriAsync(Uri uri)
    {
        var service = await this.serviceRegistry.GetAsync(serviceName) ?? throw new ServiceDiscoveryException($"There is no available service with {serviceName} name.");

        return new UriBuilder(uri)
        {
            Port = service.Port,
            Host = service.Address
        }.Uri;
    }

}
