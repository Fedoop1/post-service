using Microsoft.AspNetCore.Identity;
using PostService.Common.Types;
using PostService.Identity.Exceptions;

namespace PostService.Identity.Models.Domain
{
    public class RefreshToken : IIdentifiable
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Token { get; }
        public DateTime CreatedAt { get; }
        public DateTime ExpiresAt { get; }
        public DateTime? RevokedAt { get; private set; }
        public bool IsRevoked => this.RevokedAt.HasValue;

        public RefreshToken(User user, IPasswordHasher<User> passwordHasher, TimeSpan expirationFrame)
        {
            if (user is null) throw new InvalidUserException("User can't be null");
            if (passwordHasher is null) throw new InvalidUserException("Password hasher can't be null");

            Id = Guid.NewGuid();
            UserId = user.Id;
            CreatedAt = DateTime.Now;
            ExpiresAt = DateTime.Now + expirationFrame;
            Token = CreateRefreshToken(user, passwordHasher);
        }

        public void Revoke()
        {
            if (this.IsRevoked) throw new InvalidRefreshTokenException($"Token {this.Token} is already revoked");

            this.RevokedAt = DateTime.Now;
        }

        private static string CreateRefreshToken(User user, IPasswordHasher<User> passwordHasher) => passwordHasher
            .HashPassword(user, Guid.NewGuid().ToString("N"))
            .Replace("=", string.Empty)
            .Replace("+", string.Empty)
            .Replace("/", string.Empty);

    }
}
