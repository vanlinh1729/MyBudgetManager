using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupMemberRepository : GenericRepository<GroupMember>, IGroupMemberRepositoryAsync
{
    private readonly IApplicationDbContext _context;

    public GroupMemberRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
    }
}