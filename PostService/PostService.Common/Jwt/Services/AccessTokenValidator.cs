using Microsoft.Extensions.Options;
using PostService.Common.Jwt.Types;

namespace PostService.Common.Jwt.Services;

public class AccessTokenValidator : IAccessTokenValidator
{
    private readonly IOptions<JwtOptions> jwtOptions;
    private readonly IJwtHandler jwtHandler;

    public AccessTokenValidator(IOptions<JwtOptions> jwtOptions, IJwtHandler jwtHandler)
    {
        this.jwtOptions = jwtOptions;
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
