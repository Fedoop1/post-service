using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using PostService.Common.Enums;
using PostService.Common.Extensions;
using PostService.Identity.Exceptions;
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
        private readonly IJwtHandler jwtHandler;

        public IdentityService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IPasswordHasher<User> passwordHasher, IJwtHandler jwtHandler)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.passwordHasher = passwordHasher;
            this.jwtHandler = jwtHandler;
        }

        public async Task SignUpAsync(string userName, string email, string password, Role role)
        {
            if (string.IsNullOrEmpty(email)) throw new InvalidEmailException("Email can't be null or empty");
            if (string.IsNullOrEmpty(userName)) throw new InvalidNameException("User name can't be null or empty");
            if (string.IsNullOrEmpty(password)) throw new InvalidPasswordException("Password can't be null or empty");

            if (await userRepository.GetAsync(userName) is not null)
            {
                throw new UserAlreadyInUseException($"User with UserName {userName} already exists");
            }

            var user = new User(userName, email, role);
            user.SetPassword(password, passwordHasher);

            await this.userRepository.AddAsync(user);
        }

        public async Task<JsonWebToken> SignInAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new InvalidNameException("User name can't be null or empty");
            if (string.IsNullOrEmpty(password)) throw new InvalidPasswordException("Password can't be null or empty");

            var user = await this.userRepository.GetAsync(userName);

            if (user is null) throw new UserNotExistsException($"User with UserName {userName} doesn't exist");

            if (!user.VerifyPassword(password, passwordHasher))
                throw new InvalidCredentialException("Invalid password");

            var accessToken = await this.jwtHandler.CreateAccessToken(user.Id, new Dictionary<string, string>()
            {
                ["Role"] = user.Role.ConvertToString()
            });

            var refreshToken = new RefreshToken(user, passwordHasher);
            await this.refreshTokenRepository.AddAsync(refreshToken);

            return new JsonWebToken(accessToken, refreshToken);
        }

        // TODO: Add change password method
    }
}
