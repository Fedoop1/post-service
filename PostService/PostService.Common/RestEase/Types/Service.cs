namespace PostService.Common.RestEase.Types;

public record Service
{
    public string Name { get; set; }
    public string Scheme { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
}
