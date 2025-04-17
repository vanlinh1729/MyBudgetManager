using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class DebtAndLoanRepositoryAsync : GenericRepositoryAsync<DebtAndLoan>, IDebtAndLoanRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public DebtAndLoanRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}