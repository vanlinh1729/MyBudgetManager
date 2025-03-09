using System.Collections;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Queries;

public class GetAllTransactionsByCategoryIdQuery: IRequest<ApiResponse<IEnumerable<Transaction>>>
{
    public Guid CategoryId { get; set; }
}

internal class GetAllTransactionsByCategoryIdQueryHandler : IRequestHandler<GetAllTransactionsByCategoryIdQuery, ApiResponse<IEnumerable<Transaction>>>
{
    private readonly ITransactionRepositoryAsync _transactionRepository;

    public GetAllTransactionsByCategoryIdQueryHandler(ITransactionRepositoryAsync transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public async Task<ApiResponse<IEnumerable<Transaction>>> Handle(GetAllTransactionsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetTransactionsByCategoryId(request.CategoryId);
        if (transaction==null)
        {
            throw new ApiException("Transaction cannot found");
        }

        return new ApiResponse<IEnumerable<Transaction>>(transaction);
    }
}