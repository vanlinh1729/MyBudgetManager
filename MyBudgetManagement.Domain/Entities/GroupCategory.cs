using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class GroupCategory: AuditableBaseEntity
{
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public CategoryType CategoryType { get; set; }
    public Guid GroupBalanceId { get; set; }
    
    //nav props
    public virtual GroupBalance GroupBalance { get; set; }
    public virtual ICollection<GroupTransaction> GroupTransactions { get; set; } = new List<GroupTransaction>();
}