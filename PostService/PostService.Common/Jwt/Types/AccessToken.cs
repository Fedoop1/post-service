using PostService.Common.Enums;

namespace PostService.Common.Jwt.Types;

public record AccessToken(Guid Id, string Token, long Expires, Role Role, IDictionary<string, string> Claims);

