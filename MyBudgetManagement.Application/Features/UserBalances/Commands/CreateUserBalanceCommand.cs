using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserBalances.Commands;

public class CreateUserBalanceCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
}

internal class CreateUserBalanceCommandHandler : IRequestHandler<CreateUserBalanceCommand, ApiResponse<string>>
{
    private readonly IUserBalanceRepositoryAsync _userBalanceRepository;

    public CreateUserBalanceCommandHandler(IUserBalanceRepositoryAsync userBalanceRepository)
    {
        _userBalanceRepository = userBalanceRepository;
    }
    public async Task<ApiResponse<string>> Handle(CreateUserBalanceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}