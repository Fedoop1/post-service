using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public interface IEventHandler<in TEvent> where  TEvent: IEvent
{
    Task HandleAsync(TEvent @event);
}
