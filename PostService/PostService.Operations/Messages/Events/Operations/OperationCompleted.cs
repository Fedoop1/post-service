using PostService.Common.RabbitMq.Types;
using PostService.Common.Types;

namespace PostService.Operations.Messages.Events.Operations;

[MessageNamespace("operations")]
public record OperationCompleted(Guid Id, Guid UserId, string Name) : IEvent;
