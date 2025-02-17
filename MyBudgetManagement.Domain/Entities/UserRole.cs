using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class UserRole : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}