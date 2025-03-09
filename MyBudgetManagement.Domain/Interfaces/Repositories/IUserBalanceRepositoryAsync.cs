using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IUserBalanceRepositoryAsync : IGenericRepositoryAsync<UserBalance>
{
    Task<UserBalance> GetUserBalanceByUserId(Guid userId);
}