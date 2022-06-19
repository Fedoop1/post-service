using Consul;

namespace PostService.Common.Consul.Types;

public class ConsulServiceRegistry : IConsulServiceRegistry
{
    private readonly IConsulClient consulClient;
    private readonly IDictionary<string, ISet<string>> usedServices = new Dictionary<string, ISet<string>>();

    public ConsulServiceRegistry(IConsulClient consulClient)
    {
        this.consulClient = consulClient;
    }

    public async Task<AgentService?> GetAsync(string name)
    {
        var services = (await this.consulClient.Agent.Services()).Response;

        if (!services.Any()) return null;

        var filteredServices = FilterServices(services, name);

        if (!filteredServices.Any())
        {
            return null;
        }
        else if (!usedServices.ContainsKey(name))
        {
            usedServices[name] = new HashSet<string>();
        }
        else if (usedServices[name].Count == filteredServices.Count)
        {
            usedServices[name].Clear();
        }

        return GetService(filteredServices, name);
    }

    private AgentService GetService(IList<AgentService> source, string serviceName)
    {
        AgentService service;
        var unusedServices = source.Where(service => !this.usedServices[serviceName].Contains(service.ID));

        if (unusedServices.Any())
        {
            service = unusedServices.First();
        }
        else
        {
            usedServices[serviceName].Clear();
            service = source.First();
        }

        usedServices[serviceName].Add(service.ID);
        return service;
    }

    private static IList<AgentService> FilterServices(IDictionary<string, AgentService> source, string serviceName) =>
        source.Where(service => service.Key.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
            .Select(service => service.Value).ToList();
}
