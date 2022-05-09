using PostService.Common.Enums;
using PostService.Common.Jwt.Types;
using PostService.Common.Types;
using PostService.Identity.Models.Domain;

namespace PostService.Identity.Services;

[Injectable(Scope.Singleton)]
public interface ITokenService
{
    public Task<RefreshToken> GetRefreshToken(User user);
    public Task<AccessToken> GetAccessToken(string refreshToken);
    public Task RevokeRefreshToken(string refreshToken);
}
