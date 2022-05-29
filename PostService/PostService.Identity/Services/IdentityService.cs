using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using PostService.Common.Enums;
using PostService.Common.Jwt.Types;
using PostService.Common.RabbitMq.Types;
using PostService.Identity.Exceptions;
using PostService.Identity.Messages.Event;
using PostService.Identity.Models.Domain;
using PostService.Identity.Repositories.Interfaces;

namespace PostService.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IBusPublisher busPublisher;

        public IdentityService(
            IUserRepository userRepository, 
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher,
            IBusPublisher busPublisher)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.passwordHasher = passwordHasher;
            this.busPublisher = busPublisher;
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
            await this.busPublisher.PublishAsync(new SignUp(user.Id, email, role));
        }

        public async Task<JsonWebToken> SignInAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new InvalidCredentialException("User name can't be null or empty");
            if (string.IsNullOrEmpty(password)) throw new InvalidCredentialException("Password can't be null or empty");

            var user = await this.userRepository.GetAsync(userName);

            if (user is null) throw new InvalidUserException($"User with UserName {userName} doesn't exist");

            if (!user.VerifyPassword(password, passwordHasher))
                throw new InvalidCredentialException("Invalid password");

            var refreshToken = await this.tokenService.GetRefreshTokenAsync(user);
            var accessToken = await this.tokenService.GetAccessTokenAsync(refreshToken.Token);

            var jwt = new JsonWebToken(accessToken, refreshToken);

            await this.busPublisher.PublishAsync(new SignIn(user.Id));
            return jwt;
        }

        public Task ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            // TODO: Add change password logic
            throw new NotImplementedException();
        }

        public async Task SignOutAsync(Guid userId, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidAccessTokenException("Access token can't be null or empty");

            var user = await this.userRepository.GetAsync(userId);

            if (user is null) throw new InvalidUserException($"User with id {user} doesn't exist");

            var refreshToken = await this.tokenService.GetRefreshTokenAsync(user);

            await this.tokenService.RevokeRefreshTokenAsync(refreshToken.Token);
            await this.tokenService.RevokeAccessTokenAsync(accessToken);
            await this.busPublisher.PublishAsync(new SignOut(userId));
        }
    }
}
