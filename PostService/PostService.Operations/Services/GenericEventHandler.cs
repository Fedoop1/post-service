using PostService.Common.RabbitMq.Types;
using PostService.Common.Types;
using PostService.Operations.Models.Domain;

namespace PostService.Operations.Services;

public class GenericEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
{
    private readonly IOperationPublisher operationPublisher;
    private readonly IOperationStorage operationStorage;

    public GenericEventHandler(IOperationPublisher operationPublisher, IOperationStorage operationStorage)
    {
        this.operationPublisher = operationPublisher;
        this.operationStorage = operationStorage;
    }

    public async Task HandleAsync(TEvent @event)
    {
        switch (@event)
        {
            case IRejectEvent rejectEvent:
            {
                var operation = new Operation()
                { Id = new Guid(), Reason = rejectEvent.Reason, State = OperationState.Rejected };

                await this.operationStorage.SetAsync(operation);
                await this.operationPublisher.PublishRejectedAsync(operation);
                return;
            }
            case IPendingEvent:
            {
                var operation = new Operation()
                    { Id = new Guid(), State = OperationState.Pending };

                await this.operationStorage.SetAsync(operation);
                await this.operationPublisher.PublishPendingAsync(operation);
                return;
            }
            case IEvent:
            {
                var operation = new Operation()
                    { Id = new Guid(), State = OperationState.Completed };

                await this.operationStorage.SetAsync(operation);
                await this.operationPublisher.PublishCompletedAsync(operation);
                return;
            }
        }
    }
}
