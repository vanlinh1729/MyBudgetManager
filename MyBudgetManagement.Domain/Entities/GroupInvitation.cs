using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class GroupInvitation : BaseEntity   
{
    public Guid GroupId { get; set; }
    public Guid InviterId { get; set; }
    public Guid InviteeId { get; set; }
    public string? Message { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
    
    //nav props
    public virtual Group Group { get; set; }
    public virtual User Inviter { get; set; }
    public virtual User Invitee { get; set; }
}