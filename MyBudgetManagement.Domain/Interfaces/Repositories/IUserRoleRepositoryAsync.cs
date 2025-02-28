using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IUserRoleRepositoryAsync :  IGenericRepositoryAsync<UserRole>
{
    Task<List<UserRole>> GetUserRolesByUserIdAsync(Guid userId);
    Task<List<UserRole>> GetUserRolesByRoleIdAsync(Guid roleId);
    Task DeleteUserRoleAsync(Guid userId, Guid roleId);
}