using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IGroupRepositoryAsync : IGenericRepositoryAsync<Group>
{
    Task<IEnumerable<Group>> GetAllGroupByUserId(Guid userId);
}