using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class Loans : AuditableBaseEntity
{
    public Guid TransactionId { get; set; }
    public string BorrowerName { get; set; }
    public string Relationship { get; set; }
    public bool IsPaid { get; set; }
    
    //nav props
    public virtual Transaction Transaction { get; set; }
}