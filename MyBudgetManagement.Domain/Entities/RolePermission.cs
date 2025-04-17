using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    //nav props
    public virtual Role Role { get; set; }
    public virtual Permission Permission { get; set; }
}