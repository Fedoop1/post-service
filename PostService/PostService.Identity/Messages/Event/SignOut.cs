using PostService.Common.Types;

namespace PostService.Identity.Messages.Event;

public record SignOut(Guid userId) : IEvent;
