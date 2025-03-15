using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;
using MyBudgetManagement.Persistance.Repositories;

namespace MyBudgetManagement.Persistance;

public static class ServiceExtentions
{
    public static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        //add services
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection")
            ));

        
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IUserRepositoryAsync, UserRepository>();  
        services.AddScoped<IUserBalanceRepositoryAsync, UserBalanceRepository>();  
        services.AddScoped<IUserRoleRepositoryAsync, UserRoleRepository>();  
        services.AddScoped<ITransactionRepositoryAsync, TransactionRepository>();  
        services.AddScoped<ICategoryRepositoryAsync, CategoryRepository>();  
        services.AddScoped<IAccountProfileRepositoryAsync, AccountProfileRepository>();  
        services.AddScoped<IRoleRepositoryAsync, RoleRepository>();  
        services.AddScoped<IPermissionRepositoryAsync, PermissionRepository>();  
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();  
        services.AddScoped<IGroupBalanceRepositoryAsync, GroupBalanceRepository>();  
        services.AddScoped<IGroupRepositoryAsync, GroupRepository>();  
        services.AddScoped<IGroupMemberRepositoryAsync, GroupMemberRepository>();  
        services.AddScoped<IGroupTransactionRepositoryAsync, GroupTransactionRepository>();  
        services.AddScoped<IGroupCategoryRepositoryAsync, GroupCategoryRepository>();  
        
    }
}