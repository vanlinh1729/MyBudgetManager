using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class GroupExpense : BaseEntity
{
    public string Name { get; set; }
    public Guid GroupId { get; set; }
    public Guid GroupMemberId { get; set; }
    public DateTime ExpenseDate { get; set; }
    public decimal Amount { get; set; }
    public string? Image { get; set; }
    public GroupExpenseStatus Status { get; set; }
    public string? Note { get; set; }

    // Navigation properties
    public virtual Group Group { get; set; }
    public virtual GroupMember GroupMember { get; set; }
}