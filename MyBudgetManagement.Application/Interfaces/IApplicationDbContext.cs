
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; } 
    DbSet<UserBalance> UserBalances { get; set; } 
    DbSet<Transaction> Transactions { get; set; } 
    DbSet<Role> Roles { get; set; }
    DbSet<GroupTransaction> GroupTransactions { get; set; }
    DbSet<GroupMember> GroupMembers { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<AccountProfile> AccountProfiles { get; set; }
    Task<int> SaveChangesAsync();
}