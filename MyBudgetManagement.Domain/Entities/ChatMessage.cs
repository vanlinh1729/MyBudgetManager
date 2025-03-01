namespace MyBudgetManagement.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public Guid GroupMemberId { get; set; }
    public DateTime Timestamp { get; set; }
}