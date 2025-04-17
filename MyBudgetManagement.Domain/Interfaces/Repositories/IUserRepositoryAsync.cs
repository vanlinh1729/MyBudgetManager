using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IUserRepositoryAsync : IGenericRepositoryAsync<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> UserLogin(string email, string password);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetUserByUserBalanceAsync(Guid userBalanceId);
    Task<bool> IsEmailUniqueAsync(string email);
    Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);
    Task<User> GetUserWithDetailsAsync(Guid userId); // Includes UserBalance, Roles, etc.
}