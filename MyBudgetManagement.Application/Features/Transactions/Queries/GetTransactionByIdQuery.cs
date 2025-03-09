using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Queries;

public class GetTransactionByIdQuery : IRequest<ApiResponse<Transaction>>
{
    public Guid Id { get; set; }
}

internal class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, ApiResponse<Transaction>>
{
    private readonly ITransactionRepositoryAsync _transactionRepository;

    public GetTransactionByIdQueryHandler(ITransactionRepositoryAsync transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public async Task<ApiResponse<Transaction>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id);
        if (transaction==null)
        {
            throw new ApiException("Transaction cannot found");
        }

        return new ApiResponse<Transaction>(transaction);
    }
}