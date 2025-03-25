using System.Net;
using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.AccountProfiles.Queries;

public class GetAccountProfileOfCurrentUserQuery : IRequest<ApiResponse<AccountProfile>>
{
    public Guid UserId { get; set; }
}

internal class
    GetAccountProfileOfCurrentUserQueryHandler : IRequestHandler<GetAccountProfileOfCurrentUserQuery,
    ApiResponse<AccountProfile>>
{
    private readonly IAccountProfileRepositoryAsync _accountProfileRepository;

    public GetAccountProfileOfCurrentUserQueryHandler(IAccountProfileRepositoryAsync accountProfileRepository)
    {
        _accountProfileRepository = accountProfileRepository;
    }

    public async Task<ApiResponse<AccountProfile>> Handle(GetAccountProfileOfCurrentUserQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var accountProfile = await _accountProfileRepository.GetAccountProfileByUserIdAsync(request.UserId);
            
            if (accountProfile == null)
            {
                return new ApiResponse<AccountProfile>(null, 
                    (int)HttpStatusCode.NotFound + "AccountProfile not found for current user.");
            }

            return new ApiResponse<AccountProfile>(accountProfile, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            return new ApiResponse<AccountProfile>(null,
                (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }
    }
}