using Consul;

namespace PostService.Common.Consul.Types;

public interface IConsulServiceRegistry
{
    public Task<AgentService?> GetAsync(string name);
}
