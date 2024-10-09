using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Persistance.Context;

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
    }
}