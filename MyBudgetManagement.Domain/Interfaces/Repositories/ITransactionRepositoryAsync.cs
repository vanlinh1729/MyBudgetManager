using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface ITransactionRepositoryAsync : IGenericRepositoryAsync<Transaction>
{
    Task UpdateTransactionsCategoryToNullAsync(Guid categoryId);
    Task DeleteTransactionAsync(Guid transactionId);
    Task<IReadOnlyList<Transaction>> GetTransactionsByCategoryId(Guid catId);
    Task<IReadOnlyList<Transaction>> GetTransactionsByUserId(Guid userId);

}