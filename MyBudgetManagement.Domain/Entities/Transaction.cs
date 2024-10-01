using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Transaction : AuditableBaseEntity
{
    public Guid? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    
    //nav props
    public virtual Category Category { get; set; }
}