using Microsoft.Extensions.Options;
using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;

public class MessageNamingConventionProvider : IMessageNamingConventionProvider
{
    private readonly string @namespace;

    public MessageNamingConventionProvider(IOptions<RabbitMqOptions> options)
    {
        this.@namespace = options.Value.Namespace;
    }

    // TODO: Add message naming convention logic
    public string GetMessageName<TMessage>(TMessage message) where TMessage : IMessage
    {
        return null;
    }
}

