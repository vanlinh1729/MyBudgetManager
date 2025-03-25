using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationDbContext _dbcontext;

    public CategoryRepository(ApplicationDbContext dbContext, IApplicationDbContext context, ApplicationDbContext dbcontext) : base(dbContext)
    {
        _context = context;
        _dbcontext = dbcontext;
    }

    public async Task<IEnumerable<Category>> GetCategoriesByUserId(Guid userId)
    {
        return await _dbcontext.Categories
            .Include(c => c.UserBalance) // Include related data if needed
            .Where(x => x.UserBalance.UserId == userId)
            .AsNoTracking() // Optional: for better performance if you're only reading
            .ToListAsync();
    }
}