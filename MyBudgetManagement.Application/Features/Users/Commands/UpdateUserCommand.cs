using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MyBudgetManagement.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<ApiResponse<string>>
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResponse<string>>
        {
            private readonly IUserRepositoryAsync _userRepository;

            public UpdateUserCommandHandler(IUserRepositoryAsync userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<ApiResponse<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ApiResponse<string>("User not found.");
                }

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                await _userRepository.UpdateAsync(user);
                return new ApiResponse<string>("User updated successfully.");
            }
        }
    }
}