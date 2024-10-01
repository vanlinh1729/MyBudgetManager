using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Role : AuditableBaseEntity
{
    public string Name { get; set; }

    //nav props
    public virtual ICollection<User> Users { get; set; } = new List<User>();  // 1 role co nhieu user
}