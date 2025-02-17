using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Users.Queries;

public class GetAllUserQuery : IRequest<ApiResponse<IEnumerable<User>>>
{
    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ApiResponse<IEnumerable<User>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserRepositoryAsync _userRepository;

        public GetAllUserQueryHandler(IApplicationDbContext context, IUserRepositoryAsync userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<IEnumerable<User>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var listUser = await _userRepository.GetAllAsync();
            if (listUser == null)
            {
                throw new ApiException("User not found.");
            }

            return new ApiResponse<IEnumerable<User>>(listUser, "Data Fetched successfully");

        }
    }
}