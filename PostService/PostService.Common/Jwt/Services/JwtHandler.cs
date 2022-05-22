using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PostService.Common.Enums;
using PostService.Common.Jwt.Extensions;
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
                new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToTimestamp().ToString(), ClaimValueTypes.Integer64),
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

            return new AccessToken
            {
                Id = id,
                Token = token,
                Expires = TimeSpan.FromMinutes(jwtOptions.AccessTokenExpiration).Milliseconds,
                Role = role,
                Claims = claims ?? new Dictionary<string, string>()
            };
        }

        public AccessToken GetTokenPayload(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidAccessTokenException("Access token can't be null or empty");

            var securityTokenHandler = new JwtSecurityTokenHandler();

            securityTokenHandler.ValidateToken(accessToken,
                new TokenValidationParameters()
                {
                    IssuerSigningKey = this.signingCredentials.Key,
                    ValidateLifetime = this.jwtOptions.ValidateLifetime,
                    ValidIssuer = this.jwtOptions.Issuer,
                    ValidAudience = this.jwtOptions.ValidAudience,
                    ValidateAudience = this.jwtOptions.ValidateAudience,
                }, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken) return null;

            return new AccessToken()
            {
                Id = Guid.Parse(jwtSecurityToken.Id),
                Token = accessToken,
                Expires = jwtSecurityToken.ValidTo.ToTimestamp(),
                Claims = jwtSecurityToken.Claims.ToDictionary(c => c.Type, c => c.Value),
                Role = Enum.Parse<Role>(jwtSecurityToken.Claims.FirstOrDefault((claim) => claim.Type == ClaimTypes.Role)?.Value),
            };
        }
    }
}
