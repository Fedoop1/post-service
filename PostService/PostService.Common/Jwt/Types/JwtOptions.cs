namespace PostService.Common.Jwt.Types;

public record JwtOptions
{
    public string Issuer { get; set; } 
    public int RefreshTokenExpiration {get; set; } 
    public int AccessTokenExpiration {get; set; } 
    public string SecretKey { get; set; }
    public bool ValidateLifetime { get; set; }
}

