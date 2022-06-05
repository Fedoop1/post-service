namespace PostService.Common.RabbitMq.Types;

public interface IMessageNamingConventionProvider
{
    string GetMessageName(Type messageType);
    string GetQueueName(Type messageType);
    string GetExchangeName(Type messageType);
    string GetErrorQueueName();
    string GetErrorExchangeName();
}
