using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsActive => RevokedAt == null && !IsExpired;
    
    // Navigation property
    public User User { get; set; } 
}