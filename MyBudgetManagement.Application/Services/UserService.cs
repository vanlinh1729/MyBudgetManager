using AutoMapper;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;
using System.Text.RegularExpressions;
using System.Transactions;

namespace MyBudgetManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepositoryAsync _userRepository;
    private readonly IUserRoleRepositoryAsync _userRoleRepository;
    private readonly IRoleRepositoryAsync _roleRepository;
    private readonly IAccountProfileRepositoryAsync _accountProfileRepository;
    private readonly IUserBalanceRepositoryAsync _userBalanceRepository;
    private readonly IMapper _mapper;

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public UserService(
        IUserRepositoryAsync userRepository,
        IUserRoleRepositoryAsync userRoleRepository,
        IRoleRepositoryAsync roleRepository,
        IAccountProfileRepositoryAsync accountProfileRepository,
        IUserBalanceRepositoryAsync userBalanceRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _accountProfileRepository = accountProfileRepository;
        _userBalanceRepository = userBalanceRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken = default)
    {
        // Validate email format
        if (!EmailRegex.IsMatch(request.Email))
            throw new ApiException("Invalid email format.");

        // Check password is not empty
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ApiException("Password cannot be empty.");

        // Check if email already exists
        if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            throw new ApiException("Email already exists, please try another email.");

        // Get "User" role
        var role = await _roleRepository.GetRoleByRoleNameAsync("User")
            ?? throw new ApiException("Default role 'User' not found.");

        try
        {
            // Create new user
            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCryptHelper.HashPassword(request.Password);
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
            var accountProfileDto = _mapper.Map<AccountProfileDto>(accountProfile);
            var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(user.Id);
            var userBalanceDto =  _mapper.Map<UserBalanceDto>(userBalance);
            var listUserRoles = _mapper.Map<List<UserRoleDto>>(userRoles);
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccountProfile = accountProfileDto,
                Created = user.Created,
                CreatedBy = user.CreatedBy,
                UserBalance = userBalanceDto,
                UserRoles = listUserRoles
            };
            
            return userDto;
        }
        catch (Exception e)
        {
            throw new ApiException("Error creating user."+ e.Message);
        }
    }

    public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception e)
        {
            throw new ApiException("Error getting user by Email: "+ e.Message);
        }
       
    }
}
