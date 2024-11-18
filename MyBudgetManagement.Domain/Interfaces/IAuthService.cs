namespace MyBudgetManagement.Domain.Interfaces;

public interface IAuthService
{
    Task<string> GenerateRefreshToken(Guid userId);
    Task<string> RefreshAccessToken(string refreshTokenStr);
    Task Logout(string refreshToken);

}