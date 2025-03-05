using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IUserRepositoryAsync : IGenericRepositoryAsync<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUserBalanceAsync(Guid userBalanceId);
    Task<User?> UserLogin(string email, string password);
}