using PostService.Common.Enums;
using PostService.Common.Jwt.Types;
using PostService.Identity.Models.Domain;

namespace PostService.Identity.Services;
public interface IIdentityService
{
    public Task SignUpAsync(string userName, string email, string password, Role role);
    public Task<JsonWebToken> SignInAsync(string userName, string password);
    public Task<AccessToken> GetAccessToken(string refreshToken);
}
