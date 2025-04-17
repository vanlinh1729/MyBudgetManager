using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserRepositoryAsync : GenericRepositoryAsync<User>, IUserRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<User> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByUserBalanceAsync(Guid userBalanceId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserBalance.Id == userBalanceId);
    }

    public Task<bool> IsEmailUniqueAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserWithDetailsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> UserLogin(string email, string password)
    {
        var user = await _dbContext.Users
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