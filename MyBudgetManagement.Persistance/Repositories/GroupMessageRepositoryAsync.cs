using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupMessageRepositoryAsync : GenericRepositoryAsync<GroupMessage>, IGroupMessageRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupMessageRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}