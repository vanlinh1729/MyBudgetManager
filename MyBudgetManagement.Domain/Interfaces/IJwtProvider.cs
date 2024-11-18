using System.Security.Claims;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user, int expiryMinutes = 15);
    ClaimsPrincipal ValidateToken(string token);
}