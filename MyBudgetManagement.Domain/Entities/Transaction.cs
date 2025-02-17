using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Transaction : AuditableBaseEntity
{
    public Guid? CategoryId { get; set; }
    public Guid UserBalanceId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    
    //nav props
    public virtual Category Category { get; set; }
    public virtual UserBalance UserBalance { get; set; }
}