using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Group : AuditableBaseEntity
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Description { get; set; }
    
    //nav props
    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public virtual ICollection<GroupTransaction> GroupTransactions { get; set; } = new List<GroupTransaction>();
    public virtual GroupBalance GroupBalance { get; set; }

}