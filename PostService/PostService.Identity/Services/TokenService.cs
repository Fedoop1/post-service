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

    public TokenService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IJwtHandler jwtHandler)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.userRepository = userRepository;
        this.jwtHandler = jwtHandler;
    }

    public Task<RefreshToken> GetRefreshToken(User user)
    {
        // TODO: Add refresh token logic
        throw new NotImplementedException();
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

        return this.jwtHandler.CreateAccessToken(user.Id, user.Role);
    }

    public Task RevokeRefreshToken(string refreshToken)
    {
        // TODO: Add revoke method
        throw new NotImplementedException();
    }
}
