using PostService.Common.Types;

namespace PostService.Identity.Messages.Event;

public record AccessTokenRefreshed(Guid userId) : IEvent;