using PostService.Common.Mongo.Types;
using PostService.Operations.Models.Domain;

namespace PostService.Operations.Repositories;
public class OperationRepository : IOperationRepository
{
    private readonly IMongoRepository<Operation> operationRepository;

    public OperationRepository(IMongoRepository<Operation> operationRepository)
    {
        this.operationRepository = operationRepository;
    }

    public async Task AddAsync(Operation operation) => await this.operationRepository.AddAsync(operation);

    public async Task<Operation> GetAsync(Guid id) =>
        await this.operationRepository.FindAsync(operation => operation.Id == id);

    public async Task UpdateAsync(Operation operation) => await this.operationRepository.UpdateAsync(operation);
}
