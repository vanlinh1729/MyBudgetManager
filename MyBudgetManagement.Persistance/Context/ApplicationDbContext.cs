using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Persistance.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<DebtAndLoan> DebtAndLoans { get; set; }
    public DbSet<DebtAndLoanContact> DebtAndLoanContacts { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupExpense> GroupExpenses { get; set; }
    public DbSet<GroupExpenseShare> GroupExpenseShares { get; set; }
    public DbSet<GroupInvitation> GroupInvitations { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserBalance> UserBalances { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}