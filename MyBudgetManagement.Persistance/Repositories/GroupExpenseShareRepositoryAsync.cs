using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupExpenseShareRepositoryAsync : GenericRepositoryAsync<GroupExpenseShare>, IGroupExpenseShareRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupExpenseShareRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}