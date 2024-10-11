using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<ApiResponse<User>>
{
    public Guid Id { get; set; }
    internal class GetUserByIdQueryHanler : IRequestHandler<GetUserByIdQuery, ApiResponse<User>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserByIdQueryHanler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Users.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new ApiException("User not found.");
            }
                  
            return new ApiResponse<User>(result, "Data Fetched successfully");
        }
    }
    
}