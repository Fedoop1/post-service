namespace PostService.Common.RabbitMq.Types;

internal class RabbitMqExtensionException : Exception
{
    public RabbitMqExtensionException(string message): base(message) { }
}
