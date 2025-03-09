using MediatR;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Transactions.Commands;

public class DeleteTransactionCommand : IRequest<ApiResponse<string>>
{
    public Guid Id { get; set; }

    internal class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, ApiResponse<string>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepository;
        private readonly IApplicationDbContext _dbContext;

        public DeleteTransactionCommandHandler(
            ITransactionRepositoryAsync transactionRepository,
            IApplicationDbContext dbContext)
        {
            _transactionRepository = transactionRepository ??
                                     throw new ArgumentNullException(nameof(transactionRepository));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ApiResponse<string>> Handle(DeleteTransactionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _transactionRepository.DeleteTransactionAsync(request.Id);
                return new ApiResponse<string>("Transaction deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>($"An error occurred: {ex.Message}");
            }
        }
    }
}
