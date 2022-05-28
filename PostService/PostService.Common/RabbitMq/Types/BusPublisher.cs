using EasyNetQ;
using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public class BusPublisher : IBusPublisher
{
    private readonly IBus busClient;
    private readonly IMessageNamingConventionProvider messageNamingConventionProvider;

    public BusPublisher(IBus busClient, IMessageNamingConventionProvider messageNamingConventionProvider)
    {
        this.busClient = busClient;
        this.messageNamingConventionProvider = messageNamingConventionProvider;
    }

    public async Task SendAsync<TCommand>(TCommand command)
        where TCommand : ICommand =>
        await this.busClient.SendReceive.SendAsync(this.messageNamingConventionProvider.GetQueueName<TCommand>(),
            command);

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent =>
        await this.busClient.PubSub.PublishAsync(@event);
}
