using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using PostService.Common.Enums;
using PostService.Identity.Infrastructure.Options;
using PostService.Identity.Models.Jwt;
using PostService.Identity.Models.JWT.Interfaces;

namespace PostService.Identity.Services
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions jwtOptions;

        public JwtHandler(JwtOptions jwtOptions)
        {
            this.jwtOptions = jwtOptions;
        }

        public AccessToken CreateAccessToken(Guid id, Role role, IDictionary<string, string>? claims = null)
        {
            var jwtClaims = new List<Claim>
            {
                new(ClaimTypes.Role, role.ToString()),
                new(JwtRegisteredClaimNames.Iss, jwtOptions.Issuer),
                new(JwtRegisteredClaimNames.Sub, id.ToString("N")),
                new(JwtRegisteredClaimNames.Exp, (DateTime.Now + TimeSpan.FromMinutes(jwtOptions.AccessTokenExpiration) - DateTime.UnixEpoch).TotalSeconds.ToString()),
                new(JwtRegisteredClaimNames.Iat, (DateTime.Now - DateTime.UnixEpoch).TotalSeconds.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            };

            if (claims is not null)
            {
                jwtClaims.AddRange(claims.Select(claim => new Claim(claim.Key, claim.Value)));
            }

            // TODO: Add JwtSecurityToken logic
            return new AccessToken(id, "", 0, role, claims ?? new Dictionary<string, string>());
        }
    }
}
