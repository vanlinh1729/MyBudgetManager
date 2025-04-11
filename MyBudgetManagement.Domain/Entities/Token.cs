using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Token : BaseEntity 
{
    public Guid UserId { get; set; }
    public string TokenValue { get; set; }
    public TokenType Type { get; set; }
    public DateTime ExpireAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime RevokedAt { get; set; }
    
}