using PostService.Operations.Models.Domain;

namespace PostService.Operations.Repositories;

public interface IOperationRepository
{
    public Task AddAsync(Operation operation);
    public Task<Operation> GetAsync(Guid id);
    public Task UpdateAsync(Operation operation);
}
