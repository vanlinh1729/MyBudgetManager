using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.AccountProfiles.Queries;

public class GetAccountProfileByIdQuery : IRequest<ApiResponse<AccountProfile>?>
{
    public Guid Id { get; set; }

    internal class
        GetAccountProfileByIdQueryHandler : IRequestHandler<GetAccountProfileByIdQuery, ApiResponse<AccountProfile?>>
    {
        private readonly IAccountProfileRepositoryAsync _accountProfileRepository;

        public GetAccountProfileByIdQueryHandler(IAccountProfileRepositoryAsync accountProfileRepository)
        {
            _accountProfileRepository = accountProfileRepository;
        }

        public async Task<ApiResponse<AccountProfile?>> Handle(GetAccountProfileByIdQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var accountProfile =
                    await _accountProfileRepository.GetByIdAsync(request.Id);

                if (accountProfile == null)
                    return new ApiResponse<AccountProfile>(null,
                        (int)HttpStatusCode.NotFound + "AccountProfile not found.");

                return new ApiResponse<AccountProfile>(accountProfile, "Data fetched successfully.");
            }
            catch (Exception ex)
            {
                // Log exception nếu cần
                return new ApiResponse<AccountProfile>(null,
                    (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}