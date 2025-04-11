using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public int RoleBitMask { get; set; }

    //nav props
    public virtual ICollection<User> Users { get; set; } = new List<User>();  // 1 role co teh gan cho nhieu user
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();  // 1 role co nhieu permission
}