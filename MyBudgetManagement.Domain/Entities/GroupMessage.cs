using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class GroupMessage : BaseEntity
{
    public Guid GroupId { get; set; }
    public Guid GroupMemberId { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsPinned { get; set; }
    public string? Note { get; set; }
    public Guid? ParentMessageId { get; set; }
    //nav props
    public virtual Group Group { get; set; }
    public virtual GroupMember GroupMember { get; set; }
    public virtual GroupMessage ParentMessage { get; set; }
}