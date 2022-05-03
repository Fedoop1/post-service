using PostService.Common.Enums;

namespace PostService.Identity.Models.Jwt;

public record AccessToken(Guid Id, string Token, long Expires, Role Role, IDictionary<string, string> Claims);

