using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Category : AuditableBaseEntity
{
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public CategoryType CategoryType { get; set; }
    public Guid UserBalanceId { get; set; }
    
    //nav props
    public virtual UserBalance UserBalance { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}