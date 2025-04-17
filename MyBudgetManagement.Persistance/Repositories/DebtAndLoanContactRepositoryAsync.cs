using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class DebtAndLoanContactRepositoryAsync : GenericRepositoryAsync<DebtAndLoanContact>, IDebtAndLoanContactRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public DebtAndLoanContactRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}