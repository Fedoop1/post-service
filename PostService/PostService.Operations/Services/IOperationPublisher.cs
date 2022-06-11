using PostService.Common.Types;
using PostService.Operations.Models.Domain;

namespace PostService.Operations.Services;

[Injectable]
public interface IOperationPublisher
{
    public Task PublishCompletedAsync(Operation operation);
    public Task PublishRejectedAsync(Operation operation);
    public Task PublishPendingAsync(Operation operation);
}
