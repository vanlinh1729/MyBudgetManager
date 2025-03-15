using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class GroupTransaction : AuditableBaseEntity
{
    public Guid GroupId { get; set; }
    public Guid GroupMemberId { get; set; }
    public Guid? GroupCategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }

    public virtual GroupCategory? GroupCategory { get; set; }
    public virtual Group Group { get; set; }
    public virtual GroupMember GroupMember { get; set; }

}