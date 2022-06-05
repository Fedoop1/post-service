using PostService.Common.Types;

namespace PostService.Identity.Messages.Events;

public record SignIn(Guid UserId) : IEvent;

