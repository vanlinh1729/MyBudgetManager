using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class GroupExpenseShare : BaseEntity
{
    public Guid GroupId { get; set; }
    public Guid GroupMemberId { get; set; }
    public decimal Amount { get; set; } 
    public decimal AmountPaid { get; set; } 
    public DateTime? PaidDate { get; set; }
    public GroupExpenseShareStatus Status { get; set; }
    public string? Note { get; set; }
    
    public virtual Group? Group { get; set; }
    public virtual GroupMember? GroupMember { get; set; }
}