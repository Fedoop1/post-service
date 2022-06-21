namespace PostService.Common.LoadBalancing.Types;

public record Service
{
    public string Name { get; set; }
    public string Scheme { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
