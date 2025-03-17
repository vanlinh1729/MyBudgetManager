using MediatR;
using MyBudgetManagement.Application.Wrappers;

namespace MyBudgetManagement.Application.Features.Groups.Commands;

public class CreateGroupCommand : IRequest<ApiResponse<string>>
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Description { get; set; }
}

internal class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, ApiResponse<string>>
{
    public Task<ApiResponse<string>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}