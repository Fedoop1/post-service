namespace PostService.Common.RabbitMq.Types;

public class RabbitMqOptions
{
    public string Namespace { get; set; }
    public int Retries { get; set; }
    public TimeSpan HeartbeatInterval { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public string HostName { get; set; }
    public string VirtualHost { get; set; }
}
