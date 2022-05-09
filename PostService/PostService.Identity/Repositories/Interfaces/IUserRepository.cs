using PostService.Common.Types;
using PostService.Identity.Models.Domain;

namespace PostService.Identity.Repositories.Interfaces;

[Injectable]
public interface IUserRepository
{
    public Task AddAsync(User user);
    public Task RemoveAsync(User user);
    public Task UpdateAsync(User user);
    public Task<User> GetAsync(Guid userId);
    public Task<User> GetAsync(string userName);
}
