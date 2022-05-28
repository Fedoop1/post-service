namespace PostService.Common.RabbitMq.Types;

public class MessageException: Exception
{
    public MessageException(string message) : base(message) { }
}
