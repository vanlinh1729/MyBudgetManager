using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class AccountProfile : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Avatar { get; set; } 
    public string? Address { get; set; }
    public Gender Gender { get; set; }
    public Currencies Currency { get; set; } = Currencies.VND;
    
    //Nav props
    public User User { get; set; } // 1 user co 1 accountprofile
}
