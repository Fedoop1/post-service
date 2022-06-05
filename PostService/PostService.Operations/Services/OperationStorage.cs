using PostService.Operations.Models.Domain;

namespace PostService.Operations.Services;

public class OperationStorage : IOperationStorage
{
    public OperationStorage()
    {

    }

    public Task SetAsync(Operation operation)
    {
        throw new NotImplementedException();
    }

    public Task<Operation> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
