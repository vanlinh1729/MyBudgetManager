using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Permissions.Queries;

public class GetAllPermissionsQuery : IRequest<ApiResponse<IEnumerable<Permission>>>
{
    
}

internal class
    GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, ApiResponse<IEnumerable<Permission>>>
{
    private readonly IPermissionRepositoryAsync _permissionRepository;

    public GetAllPermissionsQueryHandler(IPermissionRepositoryAsync permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }
    public async Task<ApiResponse<IEnumerable<Permission>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var listPermissions = await _permissionRepository.GetAllAsync();
        if (listPermissions == null)
        {
            throw new ApiException("Permission not found.");
        }

        return new ApiResponse<IEnumerable<Permission>>(listPermissions, "Data Fetched successfully");

    }
}