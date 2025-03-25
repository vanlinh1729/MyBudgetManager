using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserBalanceRepository : GenericRepository<UserBalance>, IUserBalanceRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationDbContext _dbcontext;

    public UserBalanceRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
        _dbcontext = dbContext;
    }

    public async Task<UserBalance> GetUserBalanceByUserId(Guid userId)
    {
        return await _dbcontext.UserBalances
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}