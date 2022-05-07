namespace PostService.Common.Jwt.Types;

public record JwtOptions(string Issuer, int RefreshTokenExpiration, int AccessTokenExpiration, string SecurityKey, bool ValidateLifetime);

