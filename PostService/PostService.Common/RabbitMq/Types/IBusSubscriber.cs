using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public interface IBusSubscriber
{
    IBusSubscriber SubscribeCommand<TCommand>(Func<TCommand, MessageException, IRejectEvent> onError) where TCommand : ICommand;
    IBusSubscriber SubscribeEvent<TEvent>(Func<TEvent, MessageException, IRejectEvent> onError) where TEvent : IEvent;
}
