using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class GroupMember : AuditableBaseEntity
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinDate { get; set; }

    //nav props
    public virtual Group Group { get; set; }
    public virtual User User { get; set; }
}