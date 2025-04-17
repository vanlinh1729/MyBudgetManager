using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Category : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public decimal? Budget { get; set; }
    public CategoryType Type { get; set; }
    public string? Icon { get; set; }
    public CategoryLevel? Level { get; set; }
    public Period? Period { get; set; }
    
    //nav props
    public virtual User User { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<DebtAndLoan> DebtAndLoans { get; set; } = new List<DebtAndLoan>();
    
}