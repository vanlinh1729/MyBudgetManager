using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}