using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class GroupTransaction : AuditableBaseEntity
{
    public Guid GroupId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }

    public virtual Group Group { get; set; }
}