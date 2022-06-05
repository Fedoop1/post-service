using PostService.Common.Enums;
using PostService.Common.RabbitMq.Types;
using PostService.Common.Types;

namespace PostService.Operations.Messages.Events.Identity;

[MessageNamespace("identity")]
public record SignUp(Guid UserId, string Email, Role Role) : IEvent;
