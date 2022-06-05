namespace PostService.Common.RabbitMq.Types;

[AttributeUsage(AttributeTargets.Class)]
public class MessageNamespaceAttribute : Attribute
{
    public string Namespace { get; }

    public MessageNamespaceAttribute(string @namespace)
    {
        this.Namespace = @namespace;
    }
}
