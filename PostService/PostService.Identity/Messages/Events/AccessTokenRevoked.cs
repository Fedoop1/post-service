using PostService.Common.Types;

namespace PostService.Identity.Messages.Events;

public record AccessTokenRevoked(Guid UserId) : IEvent;
