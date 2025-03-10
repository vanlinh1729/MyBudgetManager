using System.Data.Entity;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserBalanceRepository : GenericRepository<UserBalance>, IUserBalanceRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationDbContext _dbcontext;

    public UserBalanceRepository(ApplicationDbContext dbContext, IApplicationDbContext context, ApplicationDbContext dbcontext) : base(dbContext)
    {
        _context = context;
        _dbcontext = dbcontext;
    }

    public async Task<UserBalance> GetUserBalanceByUserId(Guid userId)
    {
        var userBalance = await _dbcontext.UserBalances.FirstOrDefaultAsync(x => x.UserId == userId);
        return userBalance;
    }
}