
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<DebtAndLoan> DebtAndLoans { get; set; }
    DbSet<DebtAndLoanContact> DebtAndLoanContacts { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<GroupExpense> GroupExpenses { get; set; }
    DbSet<GroupExpenseShare> GroupExpenseShares { get; set; }
    DbSet<GroupInvitation> GroupInvitations { get; set; }
    DbSet<GroupMember> GroupMembers { get; set; }
    DbSet<GroupMessage> GroupMessages { get; set; }
    DbSet<Notification> Notifications { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Token> Tokens { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserBalance> UserBalances { get; set; }
}