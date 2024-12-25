using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities;

public class LendingContact : AuditableBaseEntity
{
    public string Name { get; set; }
    public string Relationship { get; set; }
    public string BankingNumber { get; set; }

}