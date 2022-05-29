using PostService.Common.Types;

namespace PostService.Identity.Messages.Event;

public record AccessTokenRevoked(Guid userId) : IEvent;
