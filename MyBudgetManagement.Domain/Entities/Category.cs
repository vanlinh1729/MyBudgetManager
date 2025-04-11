using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Category : BaseEntity
{
    public Guid UseId { get; set; }
    public string Name { get; set; }
    public decimal? Budget { get; set; }
    public CategoryType Type { get; set; }
    public string? Icon { get; set; }
    public CategoryLevel? Level { get; set; }
    public Period? Period { get; set; }
    
    //nav props
    public virtual UserBalance UserBalance { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}