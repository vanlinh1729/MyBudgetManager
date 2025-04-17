using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class GroupMember : BaseEntity
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
    public Guid? InvitationId { get; set; } 
    public bool IsGroupLeader { get; set; } // Nếu true -> Group Leader
    public GroupMemberStatus Status { get; set; } 
    public DateTime JoinDate { get; set; }
    public decimal NetBalance { get; set; }

    //nav props
    public virtual Group Group { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<GroupExpense> GroupExpenses { get; set; } = new List<GroupExpense>();
    public virtual GroupInvitation? Invitation { get; set; }


}