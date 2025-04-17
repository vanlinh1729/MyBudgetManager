using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface ITokenRepositoryAsync
{
    Task<Token> GetByToken(string token);
    Task SaveToken(Token token);
    Task<bool> RevokeToken(Guid userId, CancellationToken cancellationToken = default);
    Task<string> RefreshToken(string refreshToken);
    Task<string> RefreshAccessToken(string refreshTokenStr);
    Task<string> RevokeAndGenerateNewRefreshTokenAsync(Guid userId);
}