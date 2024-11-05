using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;

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
            user.Id = Guid.NewGuid();
            user.CreatedBy = user.Id.ToString();
            user.Created = DateTime.Now;
            user.Role = _context.Roles.FirstOrDefault(r => r.Name == "User");
            user.LastModifiedBy = user.Id.ToString();
            user.LastModified = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var accountProfile = new AccountProfile()
            {
                UserId = user.Id,
                Address = null,
                Avatar = null,
                Currency = Currencies.VND,
                DateOfBirth = null,
                Gender = Gender.Other,
                User = user,
                CreatedBy = user.Id.ToString(),
                Created = DateTime.Now,
                LastModifiedBy = user.Id.ToString(),
                LastModified = DateTime.Now

            };
            _context.AccountProfiles.Add(accountProfile);
            await _context.SaveChangesAsync(); 

            var userBalance = new UserBalance(){
                UserId = user.Id,
                User = user,
                CreatedBy = user.Id.ToString(),
                Created = DateTime.Now,
                LastModifiedBy = user.Id.ToString(),
                LastModified = DateTime.Now
            };
            _context.UserBalances.Add(userBalance);
            await _context.SaveChangesAsync(); 

            
            
            return new ApiResponse<Guid>(user.Id, "User created successfully");

            //logic
            
        }
    }
}