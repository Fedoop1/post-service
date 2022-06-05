using System.Reflection;
using EasyNetQ;
using Microsoft.Extensions.Options;

namespace PostService.Common.RabbitMq.Types;

public class MessageNamingConventionProvider : Conventions, IMessageNamingConventionProvider
{
    private readonly string @namespace;

    public MessageNamingConventionProvider(IOptions<RabbitMqOptions> options, DefaultTypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
    {
        this.@namespace = options.Value.Namespace;

        this.QueueNamingConvention = (messageType, _) => GetQueueName(messageType);
        this.ExchangeNamingConvention = messageType => GetNamespace(messageType).ToLowerInvariant();
        this.ErrorQueueNamingConvention = _ => GetErrorQueueName();
        this.ErrorExchangeNamingConvention = _ => GetErrorExchangeName();
    }

    public string GetMessageName(Type messageType) => this.FromPascalToUnderscores(messageType.Name);

    public string GetQueueName(Type messageType)
    {
        var messageNamespace = this.GetNamespace(messageType);
        var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

        var messageName = FromPascalToUnderscores(messageType.Name).ToLowerInvariant();

        return $"{assemblyName}\\{messageNamespace}_{messageName}";
    }

    public string GetExchangeName(Type messageType) => GetNamespace(messageType).ToLowerInvariant();

    public string GetErrorQueueName() => $"{@namespace}.error";
    public string GetErrorExchangeName() => $"{@namespace}.exchange-error";

    private string GetNamespace(Type messageType) =>
        messageType.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace ?? @namespace;

    private string FromPascalToUnderscores(string text) => string.Concat(
    text.Select((@char, index) =>
        index > 0 && char.IsUpper(@char) ? "_" + @char : @char.ToString())).ToLowerInvariant();
}

