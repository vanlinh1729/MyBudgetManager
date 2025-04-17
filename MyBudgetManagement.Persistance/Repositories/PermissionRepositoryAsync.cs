using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class PermissionRepositoryAsync : GenericRepositoryAsync<Permission>, IPermissionRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public PermissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
}