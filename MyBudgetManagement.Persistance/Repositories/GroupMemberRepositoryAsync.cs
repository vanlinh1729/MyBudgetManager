using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupMemberRepositoryAsync : GenericRepositoryAsync<GroupMember>, IGroupMemberRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupMemberRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}