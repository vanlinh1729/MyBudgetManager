using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<ApiResponse<string>>
    {
        public Guid CategoryId { get; set; } // ID của danh mục cần xóa
    }

    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<string>>
    {
        private readonly ICategoryRepositoryAsync _categoryRepository;
        private readonly ITransactionRepositoryAsync _transactionRepository;
        private readonly IApplicationDbContext _dbContext;

        public DeleteCategoryCommandHandler(
            ICategoryRepositoryAsync categoryRepository,
            ITransactionRepositoryAsync transactionRepository,
            IApplicationDbContext dbContext)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ApiResponse<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // Bắt đầu một transaction
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // Kiểm tra xem danh mục có tồn tại không
                    var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
                    if (category == null)
                    {
                        return new ApiResponse<string>("Category not found.");
                    }

                    // Xóa danh mục
                    await _categoryRepository.DeleteAsync(category);

                    // Cập nhật tất cả các giao dịch liên quan đến danh mục này thành null
                    await _transactionRepository.UpdateTransactionsCategoryToNullAsync(request.CategoryId);

                    // Commit transaction nếu mọi thứ thành công
                    await transaction.CommitAsync(cancellationToken);

                    return new ApiResponse<string>("Category deleted successfully and related transactions updated.");
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync(cancellationToken);
                    return new ApiResponse<string>($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}