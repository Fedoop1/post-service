using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PostService.Common.Enums;
using PostService.Common.Jwt.Types;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PostService.Common.Jwt.Services
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions jwtOptions;
        private readonly SigningCredentials signingCredentials;

        public JwtHandler(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
            this.signingCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtOptions.SecretKey)),
                    SecurityAlgorithms.HmacSha256);
        }

        public AccessToken CreateAccessToken(Guid id, Role role, IDictionary<string, string>? claims = null)
        {
            var expires = DateTime.Now + TimeSpan.FromMinutes(jwtOptions.AccessTokenExpiration);

            var jwtClaims = new List<Claim>
            {
                new(ClaimTypes.Role, role.ToString()),
                new(JwtRegisteredClaimNames.Sub, id.ToString("N")),
                new(JwtRegisteredClaimNames.Iat, (DateTime.Now - DateTime.UnixEpoch).TotalSeconds.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            };

            if (claims is not null)
            {
                jwtClaims.AddRange(claims.Select(claim => new Claim(claim.Key, claim.Value)));
            }

            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                expires: expires,
                claims: jwtClaims,
                signingCredentials: this.signingCredentials,
                notBefore: DateTime.Now);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AccessToken(id, token, TimeSpan.FromMinutes(jwtOptions.AccessTokenExpiration).Milliseconds, role,
                claims ?? new Dictionary<string, string>());
        }
    }
}
