using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.DTOs
{
    public class CategoryWithTotalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainingBudget { get; set; }
        public CategoryType CategoryType { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    }
}