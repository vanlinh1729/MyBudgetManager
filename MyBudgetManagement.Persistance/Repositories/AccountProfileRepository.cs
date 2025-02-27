using System.Data.Entity;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class AccountProfileRepository : GenericRepository<AccountProfile> , IAccountProfileRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationDbContext _dbcontext;

    public AccountProfileRepository(ApplicationDbContext dbContext, IApplicationDbContext context, ApplicationDbContext dbcontext) : base(dbContext)
    {
        _context = context;
        _dbcontext = dbcontext;
    }

    public async Task<AccountProfile?> GetAccountProfileByUserIdAsync(Guid userId)
    {
        return await _dbcontext.AccountProfiles.FirstOrDefaultAsync(x=>x.UserId == userId);
    }
}             