using AutoMapper;
using MediatR;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;

namespace MyBudgetManagement.Application.Features.Users.Queries;

public class GetUserByEmailQuery: IRequest<ApiResponse<UserDto>>
{
    public string Email { get; set; } = string.Empty;
}

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ApiResponse<UserDto>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<UserDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var userDto = await _userService.GetUserByEmailAsync(request.Email, cancellationToken);
        return new ApiResponse<UserDto>(userDto);
    }
}