using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PostService.Common.Jwt.Types;
using PostService.Identity.Exceptions;
using PostService.Identity.Models.Domain;
using PostService.Identity.Repositories.Interfaces;

namespace PostService.Identity.Services;

public class TokenService : ITokenService
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IUserRepository userRepository;
    private readonly IJwtHandler jwtHandler;
    private readonly IPasswordHasher<User> userPasswordHasher;
    private readonly JwtOptions jwtOptions;

    public TokenService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IJwtHandler jwtHandler, IPasswordHasher<User> userPasswordHasher, IOptions<JwtOptions> jwtOptions)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.userRepository = userRepository;
        this.jwtHandler = jwtHandler;
        this.userPasswordHasher = userPasswordHasher;
        this.jwtOptions = jwtOptions.Value;
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(User user)
    {
        if (user is null) throw new InvalidUserException("User can't be null");

        var userRefreshToken = await this.refreshTokenRepository.GetAsync(user.Id);

        if (userRefreshToken is not null && !userRefreshToken.IsRevoked) return userRefreshToken;

        var refreshToken = new RefreshToken(user, this.userPasswordHasher, TimeSpan.FromSeconds(this.jwtOptions.RefreshTokenExpiration));
        await this.refreshTokenRepository.AddAsync(refreshToken);

        return refreshToken;
    }

    public async Task<AccessToken> GetAccessTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new InvalidRefreshTokenException("Refresh token can't be null or empty");

        var token = await this.refreshTokenRepository.GetAsync(refreshToken);

        if (token is null) throw new InvalidRefreshTokenException($"Refresh token {refreshToken} doesn't exist");
        if (token.IsRevoked) throw new InvalidRefreshTokenException($"Refresh token {refreshToken} was revoked");

        var user = await this.userRepository.GetAsync(token.UserId);

        if (user is null) throw new InvalidUserException($"User with id {token.UserId} doesn't exist");

        return this.jwtHandler.CreateAccessToken(user.Id, user.Role);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new InvalidRefreshTokenException("Refresh token can't be null or empty");

        var refreshTokenInstance = await this.refreshTokenRepository.GetAsync(refreshToken);

        if (refreshTokenInstance is null)
            throw new InvalidRefreshTokenException($"Refresh token {refreshTokenInstance} doesn't exist");

        refreshTokenInstance.Revoke();

        await this.refreshTokenRepository.UpdateAsync(refreshTokenInstance);
    }

    public async Task RevokeAccessTokenAsync(string assesToken)
    {
        // TODO: Add redis cache here mark access token as revoked
        throw new NotImplementedException();
    }

    public AccessToken GetTokenPayload(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            throw new InvalidAccessTokenException("Access token can't be null or empty");

        return jwtHandler.GetTokenPayload(accessToken);
    }
}
