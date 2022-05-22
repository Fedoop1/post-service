using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;

public interface IBusPublisher
{
    Task SendAsync<TCommand>(TCommand command) where TCommand: ICommand;
    Task PublishAsync<TEvent>(TEvent @event) where  TEvent: IEvent;
}

