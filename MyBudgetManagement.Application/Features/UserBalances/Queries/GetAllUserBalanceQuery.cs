using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserBalances.Queries;

public class GetAllUserBalanceQuery : IRequest<ApiResponse<IEnumerable<UserBalance>>>
{
    internal class GetAllUserBalanceQueryHandler : IRequestHandler<GetAllUserBalanceQuery, ApiResponse<IEnumerable<UserBalance>>>
    {
        private readonly IUserBalanceRepositoryAsync _userBalanceRepository;

        public GetAllUserBalanceQueryHandler(IUserBalanceRepositoryAsync userBalanceRepository)
        {
            _userBalanceRepository = userBalanceRepository;
        }
        public async Task<ApiResponse<IEnumerable<UserBalance>>> Handle(GetAllUserBalanceQuery request, CancellationToken cancellationToken)
        {
            var listUserBalance = await _userBalanceRepository.GetAllAsync();
            if (listUserBalance == null)
            {
                throw new ApiException("User not found.");
            }

            return new ApiResponse<IEnumerable<UserBalance>>(listUserBalance, "Data Fetched successfully");
        }
    }
    
}