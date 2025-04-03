using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<ApiResponse<string>>
    {
        public Guid? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        
        // Thêm UserId để handler có thể lấy UserBalance
        public Guid UserId { get; set; } 
    }

    internal class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, ApiResponse<string>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepository;
        private readonly IUserBalanceRepositoryAsync _userBalanceRepository;
        private readonly IApplicationDbContext _dbContext;
        private readonly ICategoryRepositoryAsync _categoryRepository;
        private readonly IFileStorageService _fileStorageService;


        public CreateTransactionCommandHandler(ITransactionRepositoryAsync transactionRepository, IUserBalanceRepositoryAsync userBalanceRepository, IApplicationDbContext dbContext, ICategoryRepositoryAsync categoryRepository, IFileStorageService fileStorageService)
        {
            _transactionRepository = transactionRepository;
            _userBalanceRepository = userBalanceRepository;
            _dbContext = dbContext;
            _categoryRepository = categoryRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<ApiResponse<string>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = null;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var userBalance = await _userBalanceRepository.GetUserBalanceByUserId(request.UserId);
                    if (userBalance == null)
                    {
                        throw new ApiException("UserBalance not found.");
                    }

                    Category category = null;
                    if (request.CategoryId.HasValue)
                    {
                        category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                        if (category == null)
                        {
                            throw new ApiException("Category not found.");
                        }

                        // Kiểm tra loại category có phù hợp với loại transaction không
                        if ((category.CategoryType == CategoryType.Expense && request.Type == TransactionType.Income) ||
                            (category.CategoryType == CategoryType.Income && request.Type == TransactionType.Expense))
                        {
                            throw new ApiException("Transaction type does not match category type.");
                        }
                    }
                    // Upload ảnh nếu có
                    if (request.ImageFile != null && request.ImageFile.Length > 0)
                    {
                        if (!ImageVerifier.IsImageFile(request.ImageFile.FileName))
                        {
                            throw new ApiException("Only image files are allowed");
                        }

                        if (request.ImageFile.Length > 5 * 1024 * 1024) // 5MB limit
                        {
                            throw new ApiException("File size cannot exceed 5MB");
                        }

                        using var stream = new MemoryStream();
                        await request.ImageFile.CopyToAsync(stream, cancellationToken);
                        stream.Position = 0;
                        
                        imageUrl = await _fileStorageService.UploadFileAsync(stream, request.ImageFile.FileName);
                    }

                    // Kiểm tra và cập nhật số dư
                    decimal newBalance;
                    if (request.Type == TransactionType.Expense)
                    {
                        /*if (userBalance.Balance < request.Amount)
                        {
                            throw new ApiException("Insufficient balance for this transaction.");
                        }*/
                        newBalance = userBalance.Balance - request.Amount;
                    }
                    else // Income
                    {
                        newBalance = userBalance.Balance + request.Amount;
                    }

                    // Tạo transaction mới
                    var newTransaction = new Transaction
                    {
                        CategoryId = request.CategoryId,
                        UserBalanceId = userBalance.Id,
                        Amount = request.Amount,
                        Description = request.Description,
                        Image = imageUrl,
                        Type = request.Type,
                        Date = request.Date,
                        Created = DateTime.UtcNow,
                        CreatedBy = request.UserId.ToString(),
                        LastModifiedBy = request.UserId.ToString()
                    };

                    // Cập nhật UserBalance
                    userBalance.Balance = newBalance;
                    userBalance.LastModified = DateTime.UtcNow;
                    userBalance.LastModifiedBy = request.UserId.ToString();

                    await _transactionRepository.AddAsync(newTransaction);
                    await _userBalanceRepository.UpdateAsync(userBalance);

                    await transaction.CommitAsync(cancellationToken);
                    return new ApiResponse<string>("Transaction created successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new ApiException($"An error occurred while creating the transaction: {ex.Message}");
                }
            }
        }
    }
}
