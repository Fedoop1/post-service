using PostService.Identity.Models.Jwt;

namespace PostService.Identity.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task<RefreshToken> GetAsync(string token);
    }
}
