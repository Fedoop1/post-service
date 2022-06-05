using PostService.Common.Enums;
using PostService.Common.Types;

namespace PostService.Identity.Messages.Events;
public record SignUp(Guid UserId, string Email, Role Role) : IEvent;
