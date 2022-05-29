using PostService.Common.Jwt.Types;

namespace PostService.Common.Jwt.Services;

public class AccessTokenValidator : IAccessTokenValidator
{
    private readonly IJwtHandler jwtHandler;

    public AccessTokenValidator(IJwtHandler jwtHandler)
    {
        this.jwtHandler = jwtHandler;
    }

    public AccessTokenValidationResult ValidateToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            throw new InvalidAccessTokenException("Access token can't be null or empty");

        try
        {
            var tokenPayload = this.jwtHandler.GetTokenPayload(accessToken);
        }
        catch (Exception e)
        {
            return new AccessTokenValidationResult(false, e.Message);
        }

        return new AccessTokenValidationResult(true);

    }
}
