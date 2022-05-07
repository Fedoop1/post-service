using PostService.Common.Mongo.Types;
using PostService.Identity.Models.Domain;
using PostService.Identity.Repositories.Interfaces;

namespace PostService.Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoRepository<User> userRepository;

    public UserRepository(IMongoRepository<User> userRepository)
    {
        this.userRepository = userRepository;
    }

    public Task AddAsync(User user) => this.userRepository.AddAsync(user);

    public Task RemoveAsync(User user) => this.userRepository.RemoveAsync(user);

    public Task UpdateAsync(User user) => this.userRepository.UpdateAsync(user);

    public Task<User> GetAsync(Guid userId) => this.userRepository.FindAsync(userId);

    public Task<User> GetAsync(string userName) => this.userRepository.FindAsync((user) => user.UserName == userName);
}

