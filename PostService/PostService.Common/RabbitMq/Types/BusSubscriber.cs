using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostService.Common.Types;
using IMessage = PostService.Common.Types.IMessage;

namespace PostService.Common.RabbitMq.Types;

internal class BusSubscriber : IBusSubscriber
{
    private readonly IBus busClient;
    private readonly IMessageNamingConventionProvider messageNamingConventionProvider;
    private readonly IServiceProvider serviceProvider;
    private readonly RabbitMqOptions options;

    public BusSubscriber(IOptions<RabbitMqOptions> options, IBus busClient, IMessageNamingConventionProvider messageNamingConventionProvider, IServiceProvider serviceProvider)
    {
        this.options = options.Value;
        this.busClient = busClient;
        this.messageNamingConventionProvider = messageNamingConventionProvider;
        this.serviceProvider = serviceProvider;
    }

    public IBusSubscriber SubscribeCommand<TCommand>(Action<TCommand, MessageException> onError = null) where TCommand : ICommand
    {
        var commandName = this.messageNamingConventionProvider.GetMessageName<TCommand>();

        this.busClient.PubSub.SubscribeAsync<TCommand>(commandName, command =>
        {
            var commandHandler = this.serviceProvider.GetService<ICommandHandler<TCommand>>();

            return TryHandleAsync(command, () => commandHandler.HandleAsync(command), onError);
        });


        return this;
    }


    // TODO: Add event subscription logic
    public IBusSubscriber SubscribeEvent<TEvent>(Action<TEvent, MessageException> onError = null) where TEvent : IEvent
    {
        return this;
    }

    public async Task TryHandleAsync<TMessage>(TMessage message, Func<Task> handler,
        Action<TMessage, MessageException> onError) where TMessage : IMessage
    {
        var retiesLeft = this.options.Retries;
        var messageName = message.GetType().Name;

        // TODO: Add retries logic
        try
        {
            await handler();
        }
        catch (Exception exception)
        {
            if (--retiesLeft <= 0) throw new MessageException($"Unable to handle a message: {messageName}");
        }
    }
}
