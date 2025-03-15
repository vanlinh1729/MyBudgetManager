using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupRepository : GenericRepository<Group>, IGroupRepositoryAsync
{
    private readonly IApplicationDbContext _context;

    public GroupRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
    }

    public async Task<IEnumerable<Group>> GetAllGroupByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }
}