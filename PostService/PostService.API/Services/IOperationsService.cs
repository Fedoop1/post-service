using PostService.API.Models.Operations;
using RestEase;

namespace PostService.API.Services;

public interface IOperationsService
{
    [AllowAnyStatusCode]
    [Get("operations/{id}")]
    public Task<Operation> GetAsync(Guid id);
}
