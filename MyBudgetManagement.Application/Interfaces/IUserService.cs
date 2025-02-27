using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken = default);
    Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
}