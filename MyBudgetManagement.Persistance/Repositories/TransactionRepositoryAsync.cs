using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class TransactionRepositoryAsync : GenericRepositoryAsync<Transaction>, ITransactionRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task UpdateTransactionsCategoryToNullAsync(Guid categoryId)
    {
        var transactions = await _dbContext.Transactions
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync();

        foreach (var transaction in transactions)
        {
            /*
            transaction.CategoryId = null;
        */
        }

        await _dbContext.SaveChangesAsync();
        
    }

    public async Task DeleteTransactionAsync(Guid transactionId)
    {
        var transactions = await _dbContext.Transactions
            .Where(t => t.Id == transactionId).FirstAsync();
        _dbContext.Transactions.Remove(transactions);
        await _dbContext.SaveChangesAsync();              
    }

    public async Task<IReadOnlyList<Transaction>> GetTransactionsByCategoryId(Guid catId)
    {
        var transactions = await _dbContext.Transactions
            .Where(t => t.CategoryId == catId)
            .ToListAsync();
        return transactions;
    }

    public async Task<IReadOnlyList<Transaction>> GetTransactionsByUserId(Guid userId)
    {
        return await _dbContext.Transactions
            .Include(t => t.Category)
            .Where(t => t.Category.UserId == userId)
            .OrderByDescending(t => t.Date)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(Guid categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetTotalTransactionsByCategoryAsync(Guid categoryId, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetTransactionsByUserBalanceIdAsync(Guid userBalanceId)
    {
        throw new NotImplementedException();
    }
}