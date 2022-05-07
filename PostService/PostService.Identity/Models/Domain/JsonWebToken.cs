using PostService.Common.Jwt.Types;

namespace PostService.Identity.Models.Domain;
public record JsonWebToken(AccessToken AccessToken, RefreshToken RefreshToken);
