using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Queries;

public class GetAllTransactionByUserId: IRequest<ApiResponse<IEnumerable<Transaction>>>
{
    public Guid UserId { get; set; }
}

internal class GetAllTransactionByUserIdQueryHandler : IRequestHandler<GetAllTransactionByUserId, ApiResponse<IEnumerable<Transaction>>>
{
    private readonly ITransactionRepositoryAsync _transactionRepository;

    public GetAllTransactionByUserIdQueryHandler(ITransactionRepositoryAsync transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public async Task<ApiResponse<IEnumerable<Transaction>>> Handle(GetAllTransactionByUserId request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetTransactionsByUserId(request.UserId);
        if (transaction==null)
        {
            throw new ApiException("Transaction cannot found");
        }

        return new ApiResponse<IEnumerable<Transaction>>(transaction);
    }
}