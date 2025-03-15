using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class GroupBalance : AuditableBaseEntity
{
    public Guid GroupId { get; set; }
    public decimal Balance { get; set; }
    
    //nav props
    public virtual Group Group { get; set; }
    public virtual ICollection<GroupCategory> GroupCategories { get; set; } = new List<GroupCategory>();
    public virtual ICollection<GroupTransaction> GroupTransactions { get; set; } = new List<GroupTransaction>();
}