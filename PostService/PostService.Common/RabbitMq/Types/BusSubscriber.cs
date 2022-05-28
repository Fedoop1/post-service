using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostService.Common.Types;
using IMessage = PostService.Common.Types.IMessage;

namespace PostService.Common.RabbitMq.Types;

public partial class BusSubscriber : IBusSubscriber
{
    private readonly IBus busClient;
    private readonly IMessageNamingConventionProvider messageNamingConventionProvider;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger logger;
    private readonly RabbitMqOptions options;

    public BusSubscriber(IOptions<RabbitMqOptions> options, IBus busClient, IMessageNamingConventionProvider messageNamingConventionProvider, IServiceProvider serviceProvider, ILogger<BusSubscriber> logger)
    {
        this.options = options.Value;
        this.busClient = busClient;
        this.messageNamingConventionProvider = messageNamingConventionProvider;
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    public IBusSubscriber SubscribeCommand<TCommand>(Action<TCommand, MessageException> onError = null) where TCommand : ICommand
    {
        var queueName = this.messageNamingConventionProvider.GetQueueName<TCommand>();

        this.busClient.SendReceive.ReceiveAsync<TCommand>(queueName, command =>
        {
            var commandHandler = this.serviceProvider.GetService<ICommandHandler<TCommand>>();

            return TryHandleAsync(command, () => commandHandler.HandleAsync(command), onError);
        });

        LogCommandSubscription(typeof(TCommand).Name, queueName);
        return this;
    }

    public IBusSubscriber SubscribeEvent<TEvent>(Action<TEvent, MessageException> onError = null) where TEvent : IEvent
    {
        var eventName = this.messageNamingConventionProvider.GetMessageName<TEvent>();

        this.busClient.PubSub.SubscribeAsync<TEvent>(eventName, @event =>
        {
            var eventHandler = this.serviceProvider.GetService<IEventHandler<TEvent>>();

            return TryHandleAsync(@event, () => eventHandler.HandleAsync(@event), onError);
        });


        LogEventSubscription(typeof(TEvent).Name, eventName);
        return this;
    }

    public async Task TryHandleAsync<TMessage>(TMessage message, Func<Task> handler,
        Action<TMessage, MessageException> onError, int retries = 0) where TMessage : IMessage
    {
        var messageName = message.GetType().Name;

        LogHandling(messageName);

        try
        {
            await handler();
        }
        catch (Exception exception)
        {
            LogError(messageName, exception.GetType().Name, exception.Message);

            if (--retries <= 0) throw new MessageException($"Unable to handle a message: {messageName}");

            LogRetry(messageName, this.options.RetriesInterval, retries);

            await Task.Delay(this.options.RetriesInterval);
            await TryHandleAsync(message, handler, onError, retries);
        }
    }

    [LoggerMessage(1, LogLevel.Warning, "Retry a {message} in {seconds}. Reties left: {retriesLeft}.")]
    partial void LogRetry(string message, TimeSpan seconds, int retriesLeft);

    [LoggerMessage(2, LogLevel.Information, "Handling a message {message}")]
    partial void LogHandling(string message);

    [LoggerMessage(3, LogLevel.Information, "Subscribe to a command {command}. Queue name: {queueName}")]
    partial void LogCommandSubscription(string command, string queueName);
    [LoggerMessage(4, LogLevel.Information, "Subscribe to a event {event}. Event subscription id: {subscriptionId}")]
    partial void LogEventSubscription(string @event, string subscriptionId);

    [LoggerMessage(5, LogLevel.Error, "Unable to handle a message {messageName}. An error occurred: {error}. Details: {errorMessage}")]
    partial void LogError(string messageName, string error, string errorMessage);
}
