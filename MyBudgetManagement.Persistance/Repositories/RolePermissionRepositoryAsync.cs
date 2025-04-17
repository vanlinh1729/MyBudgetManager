using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class RolePermissionRepositoryAsync : GenericRepositoryAsync<RolePermission>, IRolePermissionRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public RolePermissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}