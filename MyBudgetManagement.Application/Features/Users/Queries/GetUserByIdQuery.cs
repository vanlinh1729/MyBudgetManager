using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<ApiResponse<User>>
{
    public Guid Id { get; set; }
    internal class GetUserByIdQueryHanler : IRequestHandler<GetUserByIdQuery, ApiResponse<User>>
    {
        private readonly IUserRepositoryAsync _userRepository;

        public GetUserByIdQueryHanler(IUserRepositoryAsync userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            /*
            var result = await _context.Users.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new ApiException("User not found.");
            }
            */
                  
            /*
            return new ApiResponse<User>(result, "Data Fetched successfully");
            */
            
            try
            {
                var user = await _userRepository.GetByIdAsync(request.Id);
                
                if (user == null)
                {
                    return new ApiResponse<User>(null, (int)HttpStatusCode.NotFound +"User not found.");
                }

                return new ApiResponse<User>(user, "Data fetched successfully.");
            }
            catch (Exception ex)
            {
                // Log exception nếu cần
                return new ApiResponse<User>(null, (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
            }
            
        }
    }
    
}