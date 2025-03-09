using System.Net;
using MediatR;
using MyBudgetManagement.Application.Features.Users.Queries;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserBalances.Queries;

public class GetUserBalanceByIdQuery: IRequest<ApiResponse<UserBalance>>
{
    public Guid Id { get; set; }
    
}
internal class GetUserBalanceByIdQueryHanler : IRequestHandler<GetUserBalanceByIdQuery, ApiResponse<UserBalance>>
{
    private readonly IUserBalanceRepositoryAsync _userBalanceRepository;

    public GetUserBalanceByIdQueryHanler(IUserBalanceRepositoryAsync userBalanceRepository)
    {
        _userBalanceRepository = userBalanceRepository;
    }

    public async Task<ApiResponse<UserBalance>> Handle(GetUserBalanceByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userBalance = await _userBalanceRepository.GetByIdAsync(request.Id);
                
            if (userBalance == null)
            {
                return new ApiResponse<UserBalance>(null, (int)HttpStatusCode.NotFound +"UserBalance not found.");
            }

            return new ApiResponse<UserBalance>(userBalance, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            // Log exception nếu cần
            return new ApiResponse<UserBalance>(null, (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }    }
}