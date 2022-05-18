namespace PostService.Common.Jwt.Types;

public record AccessTokenValidationResult(bool isValid, string? errorMessage = null);
