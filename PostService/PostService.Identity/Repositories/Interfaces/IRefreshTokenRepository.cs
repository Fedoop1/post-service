using PostService.Common.Jwt.Types;
using PostService.Identity.Models.Domain;

namespace PostService.Identity.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task<RefreshToken> GetAsync(string token);
    }
}
