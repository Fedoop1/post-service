using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public class BusPublisher : IBusPublisher
{
    // TODO: Add implementation
    public BusPublisher() { }

    public Task SendAsync<TCommand>(TCommand command)
        where TCommand : ICommand => Task.CompletedTask;

    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent => Task.CompletedTask;
}
