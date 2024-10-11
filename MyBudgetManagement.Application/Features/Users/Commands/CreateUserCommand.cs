using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<ApiResponse<Guid>>
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Guid>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra người dùng có tồn tại không
            var result = await _context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
            if (result != null)
            {
                throw new ApiException("Email already exists, please try another email");
            }
            var user = _mapper.Map<User>(request);
            user.CreatedBy = user.Id.ToString();
            user.Created = DateTime.Now;
            // user.LastModifiedBy = request.UserId.ToString();
            // user.LastModified = DateTime.Now;
            return new ApiResponse<Guid>(user.Id, "AccountProfile created successfully");

            //logic
            
        }
    }
}