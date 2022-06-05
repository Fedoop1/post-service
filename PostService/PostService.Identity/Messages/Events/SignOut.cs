using PostService.Common.Types;

namespace PostService.Identity.Messages.Events;

public record SignOut(Guid UserId) : IEvent;
