﻿using PostService.Common.RabbitMq.Types;
using PostService.Common.Types;

namespace PostService.Operations.Messages.Events.Identity;

[MessageNamespace("identity")]
public record AccessTokenRevoked(Guid UserId) : IEvent;
