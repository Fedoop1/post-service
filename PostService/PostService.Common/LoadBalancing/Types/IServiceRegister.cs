using Microsoft.AspNetCore.Builder;

namespace PostService.Common.LoadBalancing.Types;

public interface IServiceRegister
{
    public void RegisterHttpClient(WebApplicationBuilder webBuilder, string serviceName);
}
