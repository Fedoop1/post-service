namespace PostService.Common.Jwt.Types;

public record AccessTokenValidationResult(bool IsValid, AccessToken? TokenPayload = null, string? ErrorMessage = null);
