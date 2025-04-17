using Microsoft.Identity.Client;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupExpenseRepositoryAsync : GenericRepositoryAsync<GroupExpense>, IGroupExpenseRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupExpenseRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}