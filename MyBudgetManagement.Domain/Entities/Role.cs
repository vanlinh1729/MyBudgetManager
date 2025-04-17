using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }

    //nav props
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}