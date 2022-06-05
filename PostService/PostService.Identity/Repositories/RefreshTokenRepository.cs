using PostService.Common.Mongo.Types;
using PostService.Identity.Models.Domain;
using PostService.Identity.Repositories.Interfaces;

namespace PostService.Identity.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IMongoRepository<RefreshToken> refreshTokenRepository;

    public RefreshTokenRepository(IMongoRepository<RefreshToken> refreshTokenRepository)
    {
        this.refreshTokenRepository = refreshTokenRepository;
    }

    public async Task AddAsync(RefreshToken token) => await refreshTokenRepository.AddAsync(token);
    public async Task RemoveAsync(RefreshToken token) => await this.refreshTokenRepository.RemoveAsync(token);
    public async Task UpdateAsync(RefreshToken token) => await refreshTokenRepository.UpdateAsync(token);
    public async Task<RefreshToken> GetAsync(string token) =>
        await refreshTokenRepository.FindAsync((refreshToken) => refreshToken.Token == token);
    public async Task<RefreshToken> GetAsync(Guid userId) =>
        await refreshTokenRepository.FindAsync((token) => token.UserId == userId);
}

