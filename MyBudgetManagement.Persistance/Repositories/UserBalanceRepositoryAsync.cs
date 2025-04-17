using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserBalanceRepositoryAsync : GenericRepositoryAsync<UserBalance>, IUserBalanceRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public UserBalanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserBalance> GetUserBalanceByUserIdAsync(Guid userId)
    {
        return await _dbContext.UserBalances
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public Task<UserBalance> GetUserBalanceWithTransactionsAsync(Guid userBalanceId)
    {
        throw new NotImplementedException();
    }

    public Task<UserBalance> GetUserBalanceWithCategoriesAsync(Guid userBalanceId)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetCurrentBalanceAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}