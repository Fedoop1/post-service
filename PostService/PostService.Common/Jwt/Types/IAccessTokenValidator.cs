namespace PostService.Common.Jwt.Types;

public interface IAccessTokenValidator
{
    AccessTokenValidationResult ValidateToken(string accessToken);
}
