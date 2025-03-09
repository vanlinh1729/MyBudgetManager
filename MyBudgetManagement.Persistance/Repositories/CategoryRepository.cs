using System.Data.Entity;
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
        var listCategories = await _context.Categories.Where(x => x.UserBalance.UserId == userId).ToListAsync();
        return listCategories;
    }
}