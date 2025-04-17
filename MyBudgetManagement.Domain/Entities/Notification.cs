using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? SenderId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public NotificationType NotificationType { get; set; }
    public RelatedEntityType RelatedEntityType { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    
    //nav props
    public virtual User User { get; set; }
    public virtual User Sender { get; set; }
}