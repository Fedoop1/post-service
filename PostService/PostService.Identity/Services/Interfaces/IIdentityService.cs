using PostService.Common.Enums;
using PostService.Identity.Models.Jwt;

namespace PostService.Identity.Services.Interfaces;
public interface IIdentityService
{
    public Task SignUpAsync(string userName, string email, string password, Role role);
    public Task<JsonWebToken> SignInAsync(string userName, string password);
    public Task<AccessToken> GetAccessToken(string refreshToken);
}
