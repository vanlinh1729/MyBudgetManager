namespace MyBudgetManagement.Domain.Common;

public abstract class AuditableBaseEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}