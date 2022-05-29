using PostService.Common.Types;

namespace PostService.Identity.Messages.Event;

public record RefreshTokenRefreshed(Guid userId) : IEvent;
