using System.Data.Entity.Core.Objects.DataClasses;
using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class DebtAndLoanContact : BaseEntity
{
    public string Name { get; set; }
    public Relationship Relationship { get; set; }
    public string BankName { get; set; }
    public string BankingNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Note { get; set; }
    
    //nav props
    // nav props
    public virtual ICollection<DebtAndLoan> DebtAndLoans { get; set; } = new List<DebtAndLoan>();

}