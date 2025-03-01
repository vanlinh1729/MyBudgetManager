
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; } 
    DbSet<UserBalance> UserBalances { get; set; } 
    DbSet<Transaction> Transactions { get; set; } 
    DbSet<Role> Roles { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    
    public DbSet<Permission> Permissions { get; set; }

    DbSet<GroupTransaction> GroupTransactions { get; set; }
    public DbSet<GroupBalance> GroupBalances { get; set; }
    DbSet<GroupMember> GroupMembers { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<AccountProfile> AccountProfiles { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    Task<int> SaveChangesAsync();
    DatabaseFacade Database { get;}
}