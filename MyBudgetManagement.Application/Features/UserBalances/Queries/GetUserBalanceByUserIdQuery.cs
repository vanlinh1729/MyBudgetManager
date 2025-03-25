using System.Net;
using MediatR;
using MyBudgetManagement.Application.Features.Users.Queries;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserBalances.Queries;

public class GetUserBalanceByUserIdQuery: IRequest<ApiResponse<UserBalance>>
{
    public Guid UserId { get; set; }
}

internal class GetUserBalanceByUserIdQueryHandler : IRequestHandler<GetUserBalanceByUserIdQuery, ApiResponse<UserBalance>>
{
    private readonly IUserBalanceRepositoryAsync _userBalanceRepository;

    public GetUserBalanceByUserIdQueryHandler(IUserBalanceRepositoryAsync userBalanceRepository)
    {
        _userBalanceRepository = userBalanceRepository;
    }

    public async Task<ApiResponse<UserBalance>> Handle(GetUserBalanceByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userBalance = await _userBalanceRepository.GetUserBalanceByUserId(request.UserId);
                
            if (userBalance == null)
            {
                return new ApiResponse<UserBalance>(null, (int)HttpStatusCode.NotFound + "UserBalance not found.");
            }

            return new ApiResponse<UserBalance>(userBalance, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserBalance>(null, (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }
    }
}