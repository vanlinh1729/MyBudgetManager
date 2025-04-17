using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepositoryAsync Users { get; }
    IUserBalanceRepositoryAsync UserBalances { get; }
    IUserRoleRepositoryAsync UserRoles { get; }
    ITransactionRepositoryAsync Transactions { get; }
    ICategoryRepositoryAsync Categories { get; }
    IDebtAndLoanRepositoryAsync DebtAndLoans { get; }
    IDebtAndLoanContactRepositoryAsync DebtAndLoanContacts { get; }
    IRoleRepositoryAsync Roles { get; }
    IRolePermissionRepositoryAsync RolePermissions { get; }
    
    IPermissionRepositoryAsync Permissions { get; }
    ITokenRepositoryAsync Tokens { get; }
    IGroupRepositoryAsync Groups { get; }
    IGroupMemberRepositoryAsync GroupMembers { get; }
    IGroupExpenseRepositoryAsync GroupExpenses { get; }
    IGroupExpenseShareRepositoryAsync GroupExpenseShares { get; }
    IGroupInvitationRepositoryAsync GroupInvitations { get; }
    INotificationRepositoryAsync Notifications { get; }
    IGroupMessageRepositoryAsync GroupMessages { get; }
    
    // Lưu thay đổi
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    // Bắt đầu transaction
    Task BeginTransactionAsync();
    
    // Commit transaction
    Task CommitTransactionAsync();
    
    // Rollback transaction
    Task RollbackTransactionAsync();
    
    
}