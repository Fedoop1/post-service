namespace PostService.Common.CORS.Types;
public record CorsOptions
{
    public string Policy { get; set; }
    public CorsPolicy[] Policies { get; set; }
}

public record CorsPolicy
{
    public string Name { get; set; }
    public string[]? AllowedOrigins { get; set; }
    public string[]? AllowedMethods { get; set; }
    public string[]? AllowedHeaders { get; set; }
    public bool? AllowAnyOrigins { get; set; }
    public bool? AllowAnyMethods { get; set; }
    public bool? AllowAnyHeaders { get; set; }
    public bool AllowCredentials { get; set; }
}
