using PostService.Common.Mongo;
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

    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetAsync(string userName)
    {
        throw new NotImplementedException();
    }
}

