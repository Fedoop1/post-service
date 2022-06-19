namespace PostService.Common.Consul.Types;

public record ConsulOptions
{
    public string ServiceAddress { get; set; }
    public int ServicePort { get; set; }
    public string ServiceName { get; set; }
    public string ClusterAddress { get; set; }
    public string ClusterPort { get; set; }
    public bool PingEnabled { get; set; }
    public string PingEndpoint { get; set; }
    public int PingInterval { get; set; }
    public int RemoveAfterInterval { get; set; }
    public int RequestRetries { get; set; }
}
