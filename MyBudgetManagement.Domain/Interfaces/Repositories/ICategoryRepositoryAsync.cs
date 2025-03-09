using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface ICategoryRepositoryAsync : IGenericRepositoryAsync<Category>
{
    Task<IEnumerable<Category>> GetCategoriesByUserId(Guid userId);
}