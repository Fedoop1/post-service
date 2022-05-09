﻿using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PostService.Common.Enums;
using PostService.Common.Jwt.Types;
using PostService.Identity.Exceptions;
using PostService.Identity.Models.Domain;
using PostService.Identity.Repositories.Interfaces;

namespace PostService.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly IPasswordHasher<User> passwordHasher;

        public IdentityService(
            IUserRepository userRepository, 
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.passwordHasher = passwordHasher;
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

            var refreshToken = await this.tokenService.GetRefreshToken(user);
            var accessToken = await this.tokenService.GetAccessToken(refreshToken.Token);

            return new JsonWebToken(accessToken, refreshToken);
        }

        public Task ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            // TODO: Add change password method
            throw new NotImplementedException();
        }
    }
}
