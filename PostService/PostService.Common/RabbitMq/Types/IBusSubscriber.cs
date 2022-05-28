using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public interface IBusSubscriber
{
    IBusSubscriber SubscribeCommand<TCommand>(Action<TCommand, MessageException> onError = null) where TCommand : ICommand;
    IBusSubscriber SubscribeEvent<TEvent>(Action<TEvent, MessageException> onError = null) where TEvent : IEvent;
}
