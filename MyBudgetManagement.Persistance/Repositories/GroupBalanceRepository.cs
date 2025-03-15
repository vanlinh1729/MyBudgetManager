using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupBalanceRepository : GenericRepository<GroupBalance>, IGroupBalanceRepositoryAsync
{
    private readonly IApplicationDbContext _context;

    public GroupBalanceRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
    }
}