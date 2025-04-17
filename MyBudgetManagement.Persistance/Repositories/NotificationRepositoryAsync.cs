using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class NotificationRepositoryAsync : GenericRepositoryAsync<Notification>, INotificationRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}