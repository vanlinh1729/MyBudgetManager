using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Features.Users.Queries;

public class GetAllUserQuery : IRequest<ApiResponse<IEnumerable<User>>>
{
    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ApiResponse<IEnumerable<User>>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<IEnumerable<User>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var listUser = await _context.Users.ToListAsync(cancellationToken);
            if (listUser == null)
            {
                throw new ApiException("User not found.");
            }

            return new ApiResponse<IEnumerable<User>>(listUser, "Data Fetched successfully");

        }
    }
}