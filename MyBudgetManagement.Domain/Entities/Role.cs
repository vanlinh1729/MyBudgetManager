using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }

    //nav props
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}