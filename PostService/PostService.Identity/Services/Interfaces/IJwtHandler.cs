using PostService.Identity.Models.Jwt;

namespace PostService.Identity.Models.JWT.Interfaces
{
    public interface IJwtHandler
    {
        public Task<AccessToken> CreateAccessToken(Guid id, IDictionary<string, string> claims);
    }
}
