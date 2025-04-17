using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Interfaces;

public interface ICategoryRepositoryAsync : IGenericRepositoryAsync<Category>
{
    Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId);
    Task<IEnumerable<Category>> GetCategoriesByUserBalanceIdAsync(Guid userBalanceId);
    Task<IEnumerable<Category>> GetCategoriesByTypeAsync(Guid userId, CategoryType type);
    Task<Category> GetCategoryWithTransactionsAsync(Guid categoryId);
    
}