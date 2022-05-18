using PostService.Common.Enums;
using PostService.Common.Jwt.Types;
using PostService.Common.Types;
using PostService.Identity.Models.Domain;

namespace PostService.Identity.Services;

[Injectable(Scope.Singleton)]
public interface ITokenService
{
    public Task<RefreshToken> GetRefreshTokenAsync(User user);
    public Task<AccessToken> GetAccessTokenAsync(string refreshToken);
    public Task RevokeRefreshTokenAsync(string refreshToken);
    public Task RevokeAccessTokenAsync(string accessToken);
    public AccessToken GetTokenPayload(string accessToken);
}
