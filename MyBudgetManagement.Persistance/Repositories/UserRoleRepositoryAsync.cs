using System.Linq.Dynamic.Core;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserRoleRepositoryAsync : GenericRepositoryAsync<UserRole>, IUserRoleRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;
    
    public UserRoleRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserRole>> GetUserRolesByUserIdAsync(Guid userId)
    {
        return await _dbContext.UserRoles.Where(x=>x.UserId==userId).ToListAsync();
    }

    public async Task<List<UserRole>> GetUserRolesByRoleIdAsync(Guid roleId)
    {
        return await _dbContext.UserRoles.Where(x=>x.RoleId==roleId).ToListAsync();
    }

    public async Task DeleteUserRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = await _dbContext.UserRoles.Where(x=>x.UserId == userId && x.UserId == roleId).FirstOrDefaultAsync();
        if (userRole != null)
        {
            _dbContext.UserRoles.Remove(userRole);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> UpdateUserRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = await _dbContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId);

        if (userRole == null)
        {
            return false;
        }

        userRole.RoleId = roleId;
        _dbContext.UserRoles.Update(userRole);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}