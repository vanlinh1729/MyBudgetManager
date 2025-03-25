using System.Net;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Queries;

public class GetAllTransactionByUserId : IRequest<ApiResponse<IEnumerable<Transaction>>>
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
        try
        {
            var transactions = await _transactionRepository.GetTransactionsByUserId(request.UserId);
            
            if (transactions == null || !transactions.Any())
            {
                return new ApiResponse<IEnumerable<Transaction>>(
                    Enumerable.Empty<Transaction>(), 
                    "No transactions found for this user."
                );
            }

            return new ApiResponse<IEnumerable<Transaction>>(transactions, "Transactions retrieved successfully.");
        }
        catch (Exception ex)
        {
            throw new ApiException($"An error occurred while retrieving transactions: {ex.Message}");
        }
    }
}