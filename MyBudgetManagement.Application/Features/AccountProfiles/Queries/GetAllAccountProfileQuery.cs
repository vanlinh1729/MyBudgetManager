using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.AccountProfiles.Queries;

public class GetAllAccountProfileQuery : IRequest<ApiResponse<IEnumerable<AccountProfile>>>
{
    internal class GetAllAccountProfileQueryHandler : IRequestHandler<GetAllAccountProfileQuery, ApiResponse<IEnumerable<AccountProfile>>>
    {
        private readonly IAccountProfileRepositoryAsync _accountProfileRepository;

        public GetAllAccountProfileQueryHandler(IAccountProfileRepositoryAsync accountProfileRepository)
        {
            _accountProfileRepository = accountProfileRepository;
        }

        public async Task<ApiResponse<IEnumerable<AccountProfile>>> Handle(GetAllAccountProfileQuery request, CancellationToken cancellationToken)
        {
            var listAccountProfiles = await _accountProfileRepository.GetAllAsync();
            if (listAccountProfiles == null)
            {
                throw new ApiException("User not found.");
            }

            return new ApiResponse<IEnumerable<AccountProfile>>(listAccountProfiles, "Data Fetched successfully");

        }
    }
}