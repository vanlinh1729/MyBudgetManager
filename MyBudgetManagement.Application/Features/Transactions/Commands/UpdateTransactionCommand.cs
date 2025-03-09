using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Commands;

public class UpdateTransactionCommand: IRequest<ApiResponse<string>>
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid UserBalanceId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
    }

    internal class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, ApiResponse<string>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepository;
        private readonly IUserBalanceRepositoryAsync _userBalanceRepository;
        private readonly ICategoryRepositoryAsync _categoryRepository;

        public UpdateTransactionCommandHandler(
            ITransactionRepositoryAsync transactionRepository,
            IUserBalanceRepositoryAsync userBalanceRepository,
            ICategoryRepositoryAsync categoryRepository)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _userBalanceRepository = userBalanceRepository ?? throw new ArgumentNullException(nameof(userBalanceRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<ApiResponse<string>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var userBalance = await _userBalanceRepository.GetByIdAsync(request.UserBalanceId);
            if (userBalance == null)
            {
                return new ApiResponse<string>("UserBalance not found.");
            }

            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                if (category == null)
                {
                    return new ApiResponse<string>("Category not found.");
                }
            }

            var transactionFound = await _transactionRepository.GetByIdAsync(request.Id);
            if (transactionFound == null)
            {
                return new ApiResponse<string>("transaction not found.");
            }



            transactionFound.CategoryId = request.CategoryId;
            transactionFound.Amount = request.Amount;
            transactionFound.Description = request.Description;
            transactionFound.Image = request.Image;
            transactionFound.Type = request.Type;
            transactionFound.Date = request.Date;
            transactionFound.LastModifiedBy = userBalance.UserId.ToString();
            transactionFound.LastModified = DateTime.UtcNow;
           
           

            await _transactionRepository.UpdateAsync(transactionFound);

            return new ApiResponse<string>("Transaction updated successfully.");
        }
    }
