using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupInvitationRepositoryAsync : GenericRepositoryAsync<GroupInvitation>, IGroupInvitationRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupInvitationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}