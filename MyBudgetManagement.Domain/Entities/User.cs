using System.Net;
using System.Security.AccessControl;
using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class User : AuditableBaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid RoleId { get; set; }
    
    //nav props
    public virtual AccountProfile AccountProfile { get; set; } //1 user co 1 accountProfile
    public virtual Role Role { get; set; } // 1user co 1 role
    public virtual UserBalance UserBalance { get; set; } // 1user co 1 role
    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
}