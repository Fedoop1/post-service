namespace PostService.Common.LoadBalancing.Types;

public record ServiceRegisterOptions
{
    public IEnumerable<Service> Services { get; set; }
}
