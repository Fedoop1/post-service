namespace PostService.Common.RestEase.Types;

public record RestEaseOptions
{
    public string LoadBalancer { get; set; }
    public IEnumerable<Service> Services { get; set; }
}
