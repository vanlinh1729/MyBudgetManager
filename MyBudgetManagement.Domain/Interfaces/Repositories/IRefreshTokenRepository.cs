using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByToken(string token);
    Task SaveToken(RefreshToken token);
    Task<bool> RevokeToken(Guid userId, CancellationToken cancellationToken = default);
    Task<string> RefreshToken(string refreshToken);
    Task<string> RefreshAccessToken(string refreshTokenStr);
    Task<string> RevokeAndGenerateNewRefreshTokenAsync(Guid userId);
}