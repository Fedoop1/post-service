using PostService.Common.Types;

namespace PostService.Common.RabbitMq.Types;
public interface ICommandHandler<in TCommand> where  TCommand: ICommand
{
    Task HandleAsync(TCommand command);
}
