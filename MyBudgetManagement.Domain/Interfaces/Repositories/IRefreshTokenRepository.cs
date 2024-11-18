using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByToken(string token);
    Task SaveToken(RefreshToken token);
    Task RevokeToken(Guid userId);
    Task<string> RefreshToken(string refreshToken);
    Task<string> RefreshAccessToken(string refreshTokenStr);
    Task<string> RevokeAndGenerateNewRefreshTokenAsync(Guid userId);
}