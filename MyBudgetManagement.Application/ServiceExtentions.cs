using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyBudgetManagement.Application;

public static class ServiceExtentions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}