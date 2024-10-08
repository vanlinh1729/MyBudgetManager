using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyBudgetManagement.Application;

public static class ServiceExtentions
{
    public static void AddApplication(this IServiceCollection services)
    {
        //add services
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}