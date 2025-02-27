using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    
    public AccountProfileDto AccountProfile { get; set; }
    public UserBalanceDto UserBalance { get; set; }
    public List<UserRoleDto> UserRoles { get; set; }
}