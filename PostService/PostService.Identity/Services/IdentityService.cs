using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using PostService.Common.Enums;
using PostService.Common.Extensions;
using PostService.Identity.Exceptions;
using PostService.Identity.Infrastructure.Options;
using PostService.Identity.Models.Domain;
using PostService.Identity.Models.Jwt;
using PostService.Identity.Models.JWT.Interfaces;
using PostService.Identity.Repositories.Interfaces;
using PostService.Identity.Services.Interfaces;

namespace PostService.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly JwtOptions jwtOptions;
        private readonly IJwtHandler jwtHandler;

        public IdentityService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IPasswordHasher<User> passwordHasher, JwtOptions jwtOptions, IJwtHandler jwtHandler)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.passwordHasher = passwordHasher;
            this.jwtOptions = jwtOptions;
            this.jwtHandler = jwtHandler;
        }

        public async Task SignUpAsync(string userName, string email, string password, Role role)
        {
            if (string.IsNullOrEmpty(email)) throw new InvalidCredentialException("Email can't be null or empty");
            if (string.IsNullOrEmpty(userName)) throw new InvalidCredentialException("User name can't be null or empty");
            if (string.IsNullOrEmpty(password)) throw new InvalidCredentialException("Password can't be null or empty");

            if (await userRepository.GetAsync(userName) is not null)
            {
                throw new InvalidUserException($"User with UserName {userName} already exists");
            }

            var user = new User(userName, email, role);
            user.SetPassword(password, passwordHasher);

            await this.userRepository.AddAsync(user);
        }

        public async Task<JsonWebToken> SignInAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new InvalidCredentialException("User name can't be null or empty");
            if (string.IsNullOrEmpty(password)) throw new InvalidCredentialException("Password can't be null or empty");

            var user = await this.userRepository.GetAsync(userName);

            if (user is null) throw new InvalidUserException($"User with UserName {userName} doesn't exist");

            if (!user.VerifyPassword(password, passwordHasher))
                throw new InvalidCredentialException("Invalid password");

            var accessToken = await this.jwtHandler.CreateAccessToken(user.Id, user.Role);

            var refreshToken = new RefreshToken(user, passwordHasher,
                TimeSpan.FromDays(this.jwtOptions.RefreshTokenExpiration));
            await this.refreshTokenRepository.AddAsync(refreshToken);

            return new JsonWebToken(accessToken, refreshToken);
        }

        
        public async Task<AccessToken> GetAccessToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new InvalidRefreshTokenException("Refresh token can't be null or empty");

            var token = await this.refreshTokenRepository.GetAsync(refreshToken);

            if (token is null) throw new InvalidRefreshTokenException($"Refresh token {refreshToken} doesn't exist");
            if (token.IsRevoked) throw new InvalidRefreshTokenException($"Refresh token {refreshToken} was revoked");

            var user = await this.userRepository.GetAsync(token.UserId);

            if (user is null) throw new InvalidUserException($"User with id {token.UserId} doesn't exist");

            return await this.jwtHandler.CreateAccessToken(user.Id,
                new Dictionary<string, string>() { ["Role"] = user.Role.ConvertToString() });
        }

        // TODO: Add change password method
        // TODO: Add revoke method
    }
}
