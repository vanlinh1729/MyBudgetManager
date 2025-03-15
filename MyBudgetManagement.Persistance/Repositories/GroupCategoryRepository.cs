using Microsoft.Identity.Client;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GroupCategoryRepository : GenericRepository<GroupCategory>, IGroupCategoryRepositoryAsync
{
    private readonly IApplicationDbContext _context;

    public GroupCategoryRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
    }
}