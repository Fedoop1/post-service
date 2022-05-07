using PostService.Common.Enums;
using PostService.Identity.Models.Jwt;

namespace PostService.Identity.Models.JWT.Interfaces
{
    public interface IJwtHandler
    {
        public AccessToken CreateAccessToken(Guid id, Role role, IDictionary<string, string>? claims = null);
    }
}
