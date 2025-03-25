using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationDbContext _dbcontext;

    public TransactionRepository(ApplicationDbContext dbContext, IApplicationDbContext context, ApplicationDbContext dbcontext) : base(dbContext)
    {
        _context = context;
        _dbcontext = dbcontext;
    }

    public async Task UpdateTransactionsCategoryToNullAsync(Guid categoryId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync();

        foreach (var transaction in transactions)
        {
            transaction.CategoryId = null;
        }

        await _context.SaveChangesAsync();
        
    }

    public async Task DeleteTransactionAsync(Guid transactionId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.Id == transactionId).FirstAsync();
        _context.Transactions.Remove(transactions);
        await _context.SaveChangesAsync();              
    }

    public async Task<IReadOnlyList<Transaction>> GetTransactionsByCategoryId(Guid catId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.CategoryId == catId)
            .ToListAsync();
        return transactions;
    }

    public async Task<IReadOnlyList<Transaction>> GetTransactionsByUserId(Guid userId)
    {
        return await _dbcontext.Transactions
            .Include(t => t.UserBalance)
            .Include(t => t.Category)
            .Where(t => t.UserBalance.UserId == userId)
            .OrderByDescending(t => t.Date)
            .AsNoTracking()
            .ToListAsync();
    }
}