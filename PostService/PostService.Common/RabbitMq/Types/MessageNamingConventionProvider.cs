using System.Reflection;
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

    public string GetMessageName<TMessage>() where TMessage : IMessage
    {
        var messageType = typeof(TMessage);

        return FromPascalToUnderscores(messageType.Name).ToLowerInvariant();
    }

    public string GetQueueName<TMessage>() where TMessage : IMessage
    {
        var messageType = typeof(TMessage);

        var messageNamespace = messageType.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace ?? @namespace;
        var assemblyName = Assembly.GetCallingAssembly().GetName().Name;
        var typeName = FromPascalToUnderscores(messageType.Name).ToLowerInvariant();

        return $"{assemblyName}_{messageNamespace}_{typeName}";
    }

    private string FromPascalToUnderscores(string text) => string.Concat(
    text.Select((@char, index) =>
        index > 0 && char.IsUpper(@char) ? "_" + @char : @char.ToString())).ToLowerInvariant();
}

