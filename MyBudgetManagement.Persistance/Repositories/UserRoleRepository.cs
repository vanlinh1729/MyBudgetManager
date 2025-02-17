using LinqKit;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    public UserRoleRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
    }
}