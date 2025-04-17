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
        services.AddScoped<IUserRepositoryAsync, UserRepositoryAsync>();  
        services.AddScoped<IUserBalanceRepositoryAsync, UserBalanceRepositoryAsync>();  
        services.AddScoped<IUserRoleRepositoryAsync, UserRoleRepositoryAsync>();  
        services.AddScoped<ITransactionRepositoryAsync, TransactionRepositoryAsync>();  
        services.AddScoped<ICategoryRepositoryAsync, CategoryRepositoryAsync>();  
        services.AddScoped<IRoleRepositoryAsync, RoleRepositoryAsync>();  
        services.AddScoped<IPermissionRepositoryAsync, PermissionRepositoryAsync>();  
        services.AddScoped<ITokenRepositoryAsync, TokenRepositoryAsync>();  
        services.AddScoped<IGroupRepositoryAsync, GroupRepositoryAsync>();  
        services.AddScoped<IGroupMemberRepositoryAsync, GroupMemberRepositoryAsync>();  
        services.AddScoped<IGroupExpenseRepositoryAsync, GroupExpenseRepositoryAsync>();  
        
    }
}