using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Queries;

public class GetAllTransactionQuery : IRequest<ApiResponse<IEnumerable<Transaction>>>
{
    
}

internal class GetAllTransactionQueryHandler : IRequestHandler<GetAllTransactionQuery, ApiResponse<IEnumerable<Transaction>>>
{
    private readonly ITransactionRepositoryAsync _transactionRepository;

    public GetAllTransactionQueryHandler(ITransactionRepositoryAsync transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public async Task<ApiResponse<IEnumerable<Transaction>>> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetAllAsync();
        if (transactions==null)
        {
            throw new ApiException("Transaction cannot found");
        }

        return new ApiResponse<IEnumerable<Transaction>>(transactions);
    }
}