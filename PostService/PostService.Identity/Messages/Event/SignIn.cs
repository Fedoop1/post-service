using PostService.Common.Types;

namespace PostService.Identity.Messages.Event;

public record SignIn(Guid userId) : IEvent;

