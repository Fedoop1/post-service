using PostService.Common.Types;
using PostService.Operations.Models.Domain;

namespace PostService.Operations.Repositories;

[Injectable]
public interface IOperationRepository
{
    public Task AddAsync(Operation operation);
    public Task<Operation> GetAsync(Guid id);
    public Task UpdateAsync(Operation operation);
}
