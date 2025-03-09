using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetManagement.Application.Exceptions;

namespace MyBudgetManagement.Application.Features.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<ApiResponse<string>>
    {
        public Guid? CategoryId { get; set; }
        public Guid UserBalanceId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
    }

    internal class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, ApiResponse<string>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepository;
        private readonly IUserBalanceRepositoryAsync _userBalanceRepository;
        private readonly ICategoryRepositoryAsync _categoryRepository;

        public CreateTransactionCommandHandler(
            ITransactionRepositoryAsync transactionRepository,
            IUserBalanceRepositoryAsync userBalanceRepository,
            ICategoryRepositoryAsync categoryRepository)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _userBalanceRepository = userBalanceRepository ?? throw new ArgumentNullException(nameof(userBalanceRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<ApiResponse<string>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var userBalance = await _userBalanceRepository.GetByIdAsync(request.UserBalanceId);
            if (userBalance == null)
            {
                throw new ApiException("UserBalance not found.");
            }

            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                if (category == null)
                {
                    throw new ApiException("Category not found.");
                }
            }

            var transaction = new Transaction
            {
                CategoryId = request.CategoryId,
                UserBalanceId = request.UserBalanceId,
                Amount = request.Amount,
                Description = request.Description,
                Image = request.Image,
                Type = request.Type,
                Date = request.Date,
                Created = DateTime.UtcNow,
                CreatedBy = userBalance.UserId.ToString(),
                LastModifiedBy = userBalance.UserId.ToString()
            };

            await _transactionRepository.AddAsync(transaction);

            return new ApiResponse<string>("Transaction created successfully.");
        }
    }
}