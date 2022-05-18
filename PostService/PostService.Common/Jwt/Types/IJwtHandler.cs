using PostService.Common.Enums;

namespace PostService.Common.Jwt.Types;

public interface IJwtHandler
{
    public AccessToken CreateAccessToken(Guid id, Role role, IDictionary<string, string>? claims = null);
    public AccessToken GetTokenPayload(string accessToken);
}

