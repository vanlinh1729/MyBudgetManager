using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Transaction : AuditableBaseEntity
{
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Image { get; set; }
    public DateTime Date { get; set; }
    public string Note { get; set; }    

    
    //nav props
    public virtual Category Category { get; set; }
    public virtual UserBalance UserBalance { get; set; }
}