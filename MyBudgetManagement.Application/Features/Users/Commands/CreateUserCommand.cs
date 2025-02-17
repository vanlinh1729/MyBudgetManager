using System.Text.RegularExpressions;
using AutoMapper;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;
using System.Transactions;

namespace MyBudgetManagement.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<ApiResponse<Guid>>
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Guid>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IUserRoleRepositoryAsync _userRoleRepository;
        private readonly IRoleRepositoryAsync _roleRepository;
        private readonly IAccountProfileRepositoryAsync _accountProfileRepository;
        private readonly IUserBalanceRepositoryAsync _userBalanceRepository;
        private readonly IMapper _mapper;

        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public CreateUserCommandHandler(
            IApplicationDbContext context,
            IUserRepositoryAsync userRepository,
            IUserRoleRepositoryAsync userRoleRepository,
            IRoleRepositoryAsync roleRepository,
            IAccountProfileRepositoryAsync accountProfileRepository,
            IUserBalanceRepositoryAsync userBalanceRepository,
            IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _accountProfileRepository = accountProfileRepository;
            _userBalanceRepository = userBalanceRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate email format
            if (!EmailRegex.IsMatch(request.Email))
                throw new ApiException("Invalid email format.");

            // Check password is not empty
            if (string.IsNullOrWhiteSpace(request.PasswordHash))
                throw new ApiException("Password cannot be empty.");

            // Check if email already exists
            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
                throw new ApiException("Email already exists, please try another email.");

            // Get "User" role
            var role = await _roleRepository.GetRoleByRoleNameAsync("User")
                ?? throw new ApiException("Default role 'User' not found.");

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // Create new user
                var user = _mapper.Map<User>(request);
                user.PasswordHash = BCryptHelper.HashPassword(request.PasswordHash);
                user.Id = Guid.NewGuid();
                user.CreatedBy = user.Id.ToString();
                user.Created = DateTime.UtcNow;
                user.LastModifiedBy = user.Id.ToString();
                user.LastModified = DateTime.UtcNow;

                await _userRepository.AddAsync(user);

                // Assign user to role
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    CreatedBy = user.Id.ToString(),
                    Created = DateTime.UtcNow,
                    LastModifiedBy = user.Id.ToString(),
                    LastModified = DateTime.UtcNow
                };
                await _userRoleRepository.AddAsync(userRole);

                // Create AccountProfile
                var accountProfile = new AccountProfile
                {
                    UserId = user.Id,
                    Currency = Currencies.VND,
                    Gender = Gender.Other,
                    CreatedBy = user.Id.ToString(),
                    Created = DateTime.UtcNow,
                    LastModifiedBy = user.Id.ToString(),
                    LastModified = DateTime.UtcNow
                };
                await _accountProfileRepository.AddAsync(accountProfile);

                // Create UserBalance
                var userBalance = new UserBalance
                {
                    UserId = user.Id,
                    CreatedBy = user.Id.ToString(),
                    Created = DateTime.UtcNow,
                    LastModifiedBy = user.Id.ToString(),
                    LastModified = DateTime.UtcNow
                };
                await _userBalanceRepository.AddAsync(userBalance);

                // Complete transaction
                transaction.Complete();

                return new ApiResponse<Guid>(user.Id, "User created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<Guid>(Guid.Empty, $"Error creating user: {ex.Message}");
            }
        }
    }
}
