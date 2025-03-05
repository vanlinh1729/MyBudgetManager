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
}