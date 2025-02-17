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
    
    //nav props
    public virtual AccountProfile AccountProfile { get; set; } //1 user co 1 accountProfile
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>(); // 1user co 1 hoac nhieu role
    public virtual UserBalance UserBalance { get; set; } // 1user co 1 ub
    
    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
}