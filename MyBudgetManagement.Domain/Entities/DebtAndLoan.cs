using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class DebtAndLoan : BaseEntity
{
    public Guid DebtContactId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public string? Image { get; set; }
    public DateTime Date { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string Note { get; set; }   
    //nav props
    public virtual Category Category { get; set; }
    public virtual DebtAndLoanContact DebtAndLoanContact { get; set; }
    
}