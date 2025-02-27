using AutoMapper;
using MediatR;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MyBudgetManagement.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<ApiResponse<UserDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<UserDto>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createUserDto = _mapper.Map<CreateUserDto>(request);
        var userDto = await _userService.CreateUserAsync(createUserDto, cancellationToken);
        return new ApiResponse<UserDto>(userDto, "User created successfully.");
    }
}