using System.Linq.Expressions;
using PostService.Common.Types;
using PostService.Identity.Models.Domain;

namespace PostService.Identity.Repositories.Interfaces;

[Injectable]
public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task UpdateAsync(RefreshToken token);
    Task<RefreshToken> GetAsync(string token);
    Task<RefreshToken> GetAsync(Guid userId);
}
