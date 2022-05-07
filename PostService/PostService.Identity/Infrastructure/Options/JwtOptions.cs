namespace PostService.Identity.Infrastructure.Options;

public record JwtOptions(string Issuer, int RefreshTokenExpiration, int AccessTokenExpiration, string SecurityKey);

