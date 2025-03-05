using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepositoryAsync
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationDbContext _dbcontext;

    public UserRepository(IApplicationDbContext dbContext,ApplicationDbContext _dbcontext) : base(_dbcontext)
    {
        _context = dbContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByUserBalanceAsync(Guid userBalanceId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserBalance.Id == userBalanceId);
    }

    public async Task<User?> UserLogin(string email, string password)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.UserBalance)
            .SingleOrDefaultAsync(u => u.Email == email);
        // Check if user exists and verify password
        if (user == null || !BCryptHelper.VerifyPassword(password, user.PasswordHash))
            return null;
        return user;
    }
}