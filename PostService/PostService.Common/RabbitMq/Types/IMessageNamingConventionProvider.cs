using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public interface IMessageNamingConventionProvider
{
    string GetMessageName<TMessage>() where TMessage : IMessage;
}
