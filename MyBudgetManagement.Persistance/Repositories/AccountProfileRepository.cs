using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class AccountProfileRepository : GenericRepository<AccountProfile>, IAccountProfileRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationDbContext _dbContext;

    public AccountProfileRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
        _dbContext = dbContext;
    }

    public async Task<AccountProfile?> GetAccountProfileByUserIdAsync(Guid userId)
    {
        return await _dbContext.AccountProfiles
            .AsNoTracking()
            .Include(ap => ap.User)
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}             