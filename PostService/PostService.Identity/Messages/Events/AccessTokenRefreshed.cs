using PostService.Common.Types;

namespace PostService.Identity.Messages.Events;

public record AccessTokenRefreshed(Guid UserId) : IEvent;