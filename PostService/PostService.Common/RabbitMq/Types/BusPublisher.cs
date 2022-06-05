using EasyNetQ;
using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public class BusPublisher : IBusPublisher
{
    private readonly IBus busClient;

    public BusPublisher(IBus busClient)
    {
        this.busClient = busClient;
    }

    public async Task SendAsync<TCommand>(TCommand command)
        where TCommand : ICommand =>
        await this.busClient.SendReceive.SendAsync(null,
            command);

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent =>
        await this.busClient.PubSub.PublishAsync(@event);
}
