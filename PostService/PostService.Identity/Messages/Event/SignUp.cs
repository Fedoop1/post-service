using PostService.Common.Enums;
using PostService.Common.Types;

namespace PostService.Identity.Messages.Event;
public record SignUp(Guid userId, string email, Role role) : IEvent;
