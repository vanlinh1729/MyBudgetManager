using AutoMapper;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;

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
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IRoleRepositoryAsync _roleRepository;
        private readonly IAccountProfileRepositoryAsync _accountProfileRepository;
        private readonly IUserBalanceRepositoryAsync _userBalanceRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IApplicationDbContext context, IUserRepositoryAsync userRepository, IRoleRepositoryAsync roleRepository, IAccountProfileRepositoryAsync accountProfileRepository, IUserBalanceRepositoryAsync userBalanceRepository, IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _accountProfileRepository = accountProfileRepository;
            _userBalanceRepository = userBalanceRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUserByEmailAsync(request.Email);
            if (result != null)
            {
                throw new ApiException("Email already exists, please try another email");
            }
            var role = await _roleRepository.GetRoleByRoleNameAsync("User");

            if (role == null)
            {
                role = new Role { Name = "User" };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync(); // Lưu role vào DB
            }
            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCryptHelper.HashPassword(request.PasswordHash);
            user.Id = Guid.NewGuid();
            user.CreatedBy = user.Id.ToString();
            user.Created = DateTime.Now;
            user.RoleId = role.Id;
            user.LastModifiedBy = user.Id.ToString();
            user.LastModified = DateTime.Now;
            await _userRepository.AddAsync(user);

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
            await _accountProfileRepository.AddAsync(accountProfile);
            user.AccountProfile = accountProfile;
            user.Role = role;

            var userBalance = new UserBalance(){
                UserId = user.Id,
                User = user,
                CreatedBy = user.Id.ToString(),
                Created = DateTime.Now,
                LastModifiedBy = user.Id.ToString(),
                LastModified = DateTime.Now
            };
            await _userBalanceRepository.AddAsync(userBalance);
            user.UserBalance = userBalance;
            await _context.SaveChangesAsync();
            return new ApiResponse<Guid>(user.Id, "User created successfully");
            
        }
    }
}