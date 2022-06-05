using PostService.Common.Types;

namespace PostService.Identity.Messages.Events;
public record RefreshTokenRevoked(Guid UserId) : IEvent;
