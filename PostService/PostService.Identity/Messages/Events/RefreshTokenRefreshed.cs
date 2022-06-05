using PostService.Common.Types;

namespace PostService.Identity.Messages.Events;

public record RefreshTokenRefreshed(Guid UserId) : IEvent;
