using PostService.Common.Enums;
using PostService.Common.Types;
using PostService.Identity.Models.Domain;

namespace PostService.Identity.Services;

[Injectable(Scope.Singleton)]
public interface IIdentityService
{
    public Task SignUpAsync(string userName, string email, string password, Role role);
    public Task<JsonWebToken> SignInAsync(string userName, string password);
    public Task ChangePassword(Guid id, string oldPassword, string newPassword);
}
