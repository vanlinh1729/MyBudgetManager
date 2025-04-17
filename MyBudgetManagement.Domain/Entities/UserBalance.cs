using System.Collections;
using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class UserBalance : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    
    //nav props
    public virtual User User { get; set; }
}