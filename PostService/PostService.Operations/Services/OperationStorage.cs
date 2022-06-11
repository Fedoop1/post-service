using PostService.Operations.Models.Domain;
using PostService.Operations.Repositories;

namespace PostService.Operations.Services;

public class OperationStorage : IOperationStorage
{
    private readonly IOperationRepository operationRepository;

    public OperationStorage(IOperationRepository operationRepository)
    {
        this.operationRepository = operationRepository;
    }

    public async Task SetAsync(Operation operation) => await this.operationRepository.AddAsync(operation);

    public async Task<Operation> GetAsync(Guid id) => await this.operationRepository.GetAsync(id);
}
