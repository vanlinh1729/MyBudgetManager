using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Permission : BaseEntity
{ 
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}