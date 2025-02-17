using System.Collections;
using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class UserBalance : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    
    //nav props
    public virtual User User { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}