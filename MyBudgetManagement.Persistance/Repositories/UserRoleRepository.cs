using System.Linq.Dynamic.Core;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    public UserRoleRepository(ApplicationDbContext dbContext, IApplicationDbContext context) : base(dbContext)
    {
        _context = context;
    }

    public async Task<List<UserRole>> GetUserRolesByUserIdAsync(Guid userId)
    {
        return await _context.UserRoles.Where(x=>x.UserId==userId).ToListAsync();
    }

    public async Task<List<UserRole>> GetUserRolesByRoleIdAsync(Guid roleId)
    {
        return await _context.UserRoles.Where(x=>x.RoleId==roleId).ToListAsync();
    }

    public async Task DeleteUserRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = await _context.UserRoles.Where(x=>x.UserId == userId && x.UserId == roleId).FirstOrDefaultAsync();
        if (userRole != null)
        {
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
}