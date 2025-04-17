using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;
using MyBudgetManagement.Persistance.Repositories;

namespace MyBudgetManagement.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IJwtProvider _jwtProvider;

    private readonly ApplicationDbContext _dbContext;
    private IUserRepositoryAsync _userRepository;
    private IUserBalanceRepositoryAsync _userBalanceRepository;
    private IUserRoleRepositoryAsync _userRoleRepository;
    private ITransactionRepositoryAsync _transactionRepository;
    private ICategoryRepositoryAsync _categoryRepository;
    private IDebtAndLoanRepositoryAsync _debtAndLoanRepository;
    private IDebtAndLoanContactRepositoryAsync _debtAndLoanContactRepository;
    private IRoleRepositoryAsync _roleRepository;
    private IRolePermissionRepositoryAsync _rolePermissionRepository;
    private IPermissionRepositoryAsync _permissionRepository;
    private ITokenRepositoryAsync _tokenRepository;
    private IGroupRepositoryAsync _groupRepository;
    private IGroupMemberRepositoryAsync _groupMemberRepository;
    private IGroupExpenseRepositoryAsync _groupExpenseRepository;
    private IGroupExpenseShareRepositoryAsync _groupExpenseShareRepository;
    private IGroupInvitationRepositoryAsync _groupInvitationRepository;
    private INotificationRepositoryAsync _notificationRepository;
    private IGroupMessageRepositoryAsync _groupMessageRepository;

    public UnitOfWork(ApplicationDbContext dbContext, IJwtProvider jwtProvider)
    {
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
    }

    public IUserRepositoryAsync Users => 
        _userRepository ??= new UserRepositoryAsync(_dbContext);
    
    public IUserBalanceRepositoryAsync UserBalances => 
        _userBalanceRepository ??= new UserBalanceRepositoryAsync(_dbContext);
    
    public IUserRoleRepositoryAsync UserRoles => 
        _userRoleRepository ??= new UserRoleRepositoryAsync(_dbContext);
    
    public ITransactionRepositoryAsync Transactions => 
        _transactionRepository ??= new TransactionRepositoryAsync(_dbContext);
    
    public ICategoryRepositoryAsync Categories => 
        _categoryRepository ??= new CategoryRepositoryAsync(_dbContext);
    
    public IDebtAndLoanRepositoryAsync DebtAndLoans => 
        _debtAndLoanRepository ??= new DebtAndLoanRepositoryAsync(_dbContext);
    
    public IDebtAndLoanContactRepositoryAsync DebtAndLoanContacts => 
        _debtAndLoanContactRepository ??= new DebtAndLoanContactRepositoryAsync(_dbContext);
    
    public IRoleRepositoryAsync Roles => 
        _roleRepository ??= new RoleRepositoryAsync(_dbContext);
    
    public IRolePermissionRepositoryAsync RolePermissions => 
        _rolePermissionRepository ??= new RolePermissionRepositoryAsync(_dbContext);
    
    public IPermissionRepositoryAsync Permissions => 
        _permissionRepository ??= new PermissionRepositoryAsync(_dbContext);
    
    public ITokenRepositoryAsync Tokens => 
        _tokenRepository ??= new TokenRepositoryAsync(_dbContext, _jwtProvider);
    
    public IGroupRepositoryAsync Groups => 
        _groupRepository ??= new GroupRepositoryAsync(_dbContext);
    
    public IGroupMemberRepositoryAsync GroupMembers => 
        _groupMemberRepository ??= new GroupMemberRepositoryAsync(_dbContext);
    
    public IGroupExpenseRepositoryAsync GroupExpenses => 
        _groupExpenseRepository ??= new GroupExpenseRepositoryAsync(_dbContext);
    
    public IGroupExpenseShareRepositoryAsync GroupExpenseShares => 
        _groupExpenseShareRepository ??= new GroupExpenseShareRepositoryAsync(_dbContext);
    
    public IGroupInvitationRepositoryAsync GroupInvitations => 
        _groupInvitationRepository ??= new GroupInvitationRepositoryAsync(_dbContext);
    
    public INotificationRepositoryAsync Notifications => 
        _notificationRepository ??= new NotificationRepositoryAsync(_dbContext);
    
    public IGroupMessageRepositoryAsync GroupMessages => 
        _groupMessageRepository ??= new GroupMessageRepositoryAsync(_dbContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}