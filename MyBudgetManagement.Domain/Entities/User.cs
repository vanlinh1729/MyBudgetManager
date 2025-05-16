using System.Net;
using System.Security.AccessControl;
using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class User : AuditableBaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; }
    public string? Avatar { get; set; }
    public Gender Gender { get; set; } 
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime LastChangePassword { get; set; }
    public AccountStatus Status { get; set; }
    public Currencies Currency { get; set; }

    //nav props
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>(); // 1user co 1 hoac nhieu role
    public virtual UserBalance UserBalance { get; set; } // 1user co 1 ub
    
    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public virtual ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<GroupInvitation> SentInvitations { get; set; } = new List<GroupInvitation>();
    public virtual ICollection<GroupInvitation> ReceivedInvitations { get; set; } = new List<GroupInvitation>();
}