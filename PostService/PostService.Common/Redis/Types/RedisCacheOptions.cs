namespace PostService.Common.Redis.Types;
public class RedisCacheOptions
{
    public string ConnectionString { get; set; }
    public string Instance { get; set; }
    public string ClientName { get; set; }
    public string Password { get; set; }
}

