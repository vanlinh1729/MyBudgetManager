using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Group : AuditableBaseEntity
{
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string Description { get; set; }
    public decimal TotalSpent { get; set; }
    
    //nav props
    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public virtual ICollection<GroupExpense> GroupExpenses { get; set; } = new List<GroupExpense>();
    public virtual ICollection<GroupInvitation> GroupInvitations { get; set; } = new List<GroupInvitation>();

}