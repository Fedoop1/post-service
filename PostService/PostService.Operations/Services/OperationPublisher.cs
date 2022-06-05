using PostService.Common.RabbitMq.Types;
using PostService.Operations.Messages.Events.Operations;
using PostService.Operations.Models.Domain;

namespace PostService.Operations.Services;

public class OperationPublisher : IOperationPublisher
{
    private readonly IBusPublisher busPublisher;

    public OperationPublisher(IBusPublisher busPublisher)
    {
        this.busPublisher = busPublisher;
    }

    public Task PublishPendingAsync(Operation operation) => busPublisher.PublishAsync(new OperationPending(operation.Id, operation.UserId, operation.Name));
    public Task PublishCompletedAsync(Operation operation) => busPublisher.PublishAsync(new OperationCompleted(operation.Id, operation.UserId, operation.Name));

    public Task PublishRejectedAsync(Operation operation) =>
        busPublisher.PublishAsync(new OperationRejected(operation.Id, operation.UserId, operation.Name,
            operation.Reason));
}
