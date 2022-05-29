using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using PostService.Common.Jwt.Extensions;
using PostService.Common.Jwt.Types;
using PostService.Common.RabbitMq.Types;
using PostService.Identity.Exceptions;
using PostService.Identity.Messages.Event;
using PostService.Identity.Models.Domain;
using PostService.Identity.Repositories.Interfaces;

namespace PostService.Identity.Services;

public class TokenService : ITokenService
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IUserRepository userRepository;
    private readonly IJwtHandler jwtHandler;
    private readonly IPasswordHasher<User> userPasswordHasher;
    private readonly IBusPublisher busPublisher;
    private readonly IDistributedCache distributedCache;
    private readonly JwtOptions jwtOptions;

    public TokenService(
        IRefreshTokenRepository refreshTokenRepository, 
        IUserRepository userRepository, 
        IJwtHandler jwtHandler, 
        IPasswordHasher<User> userPasswordHasher, 
        IOptions<JwtOptions> jwtOptions, 
        IBusPublisher busPublisher,
        IDistributedCache distributedCache)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.userRepository = userRepository;
        this.jwtHandler = jwtHandler;
        this.userPasswordHasher = userPasswordHasher;
        this.busPublisher = busPublisher;
        this.distributedCache = distributedCache;
        this.jwtOptions = jwtOptions.Value;
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(User user)
    {
        if (user is null) throw new InvalidUserException("User can't be null");

        var userRefreshToken = await this.refreshTokenRepository.GetAsync(user.Id);

        if (userRefreshToken is not null && !userRefreshToken.IsRevoked) return userRefreshToken;

        var refreshToken = new RefreshToken(user, this.userPasswordHasher, TimeSpan.FromDays(this.jwtOptions.RefreshTokenExpiration));
        await this.refreshTokenRepository.AddAsync(refreshToken);
        await this.busPublisher.PublishAsync(new RefreshTokenRefreshed(user.Id));
        return refreshToken;
    }

    public async Task<AccessToken> GetAccessTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new InvalidRefreshTokenException("Refresh token can't be null or empty");

        var token = await this.refreshTokenRepository.GetAsync(refreshToken);

        if (token is null) throw new InvalidRefreshTokenException($"Refresh token {refreshToken} doesn't exist");
        if (token.IsRevoked) throw new InvalidRefreshTokenException($"Refresh token {refreshToken} was revoked");
        if (token.ExpiresAt < DateTime.Now) throw new InvalidRefreshTokenException($"Refresh token {refreshToken} has expired");

        var user = await this.userRepository.GetAsync(token.UserId);

        if (user is null) throw new InvalidUserException($"User with id {token.UserId} doesn't exist");

        var accessToken = this.jwtHandler.CreateAccessToken(user.Id, user.Role);

        await this.distributedCache.SetStringAsync(JwtExtensions.GetAccessTokenCacheKey(accessToken.Token), "active", new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(this.jwtOptions.AccessTokenExpiration)
        });

        await this.busPublisher.PublishAsync(new AccessTokenRefreshed(user.Id));

        return accessToken;
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
        await this.busPublisher.PublishAsync(new RefreshTokenRevoked(refreshTokenInstance.UserId));
    }

    public async Task RevokeAccessTokenAsync(string assesToken)
    {
        if (string.IsNullOrEmpty(assesToken))
            throw new InvalidAccessTokenException("Access token can't be null or empty");

        var accessTokenCacheKey = JwtExtensions.GetAccessTokenCacheKey(assesToken);

        if (string.IsNullOrEmpty(
                await this.distributedCache.GetStringAsync(accessTokenCacheKey)))
            throw new InvalidAccessTokenException($"Access token {assesToken} doesn't exist");

        await this.distributedCache.RemoveAsync(accessTokenCacheKey);
    }

    public AccessToken GetTokenPayload(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            throw new InvalidAccessTokenException("Access token can't be null or empty");

        return jwtHandler.GetTokenPayload(accessToken);
    }
}
