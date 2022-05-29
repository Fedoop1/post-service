using PostService.Common.Types;

namespace PostService.Identity.Messages.Event;
public record RefreshTokenRevoked(Guid userId) : IEvent;
