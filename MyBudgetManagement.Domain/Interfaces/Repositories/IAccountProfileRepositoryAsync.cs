using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IAccountProfileRepositoryAsync : IGenericRepositoryAsync<AccountProfile>
{
    Task<AccountProfile?> GetAccountProfileByUserIdAsync(Guid userId);
}