using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Persistance.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserBalance> UserBalances { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<GroupTransaction> GroupTransactions { get; set; }
    public DbSet<GroupBalance> GroupBalances { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<AccountProfile> AccountProfiles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}