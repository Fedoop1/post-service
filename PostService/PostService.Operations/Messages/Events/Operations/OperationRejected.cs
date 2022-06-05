using PostService.Common.RabbitMq.Types;
using PostService.Common.Types;

namespace PostService.Operations.Messages.Events.Operations;

[MessageNamespace("operations")]
public record OperationRejected(Guid Id, Guid UserId, string Name, string Reason) : IEvent;
