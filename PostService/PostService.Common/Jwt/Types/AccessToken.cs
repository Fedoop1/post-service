using PostService.Common.Enums;

namespace PostService.Common.Jwt.Types;

public record AccessToken()
{
    public Guid Id { get; init; }
    public string Token { get; init; }
    public long Expires { get; init; }
    public Role Role { get; init; }
    public IDictionary<string, string> Claims { get; init; }
}

