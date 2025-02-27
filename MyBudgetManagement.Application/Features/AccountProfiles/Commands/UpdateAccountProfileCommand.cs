using AutoMapper;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.AccountProfiles.Commands;


public class UpdateAccountProfileCommand : IRequest<ApiResponse<Guid>>
{
    public Guid UserId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Avatar { get; set; } 
    public string? Address { get; set; }
    public Gender Gender { get; set; }
    public Currencies Currency { get; set; } = Currencies.VND;

    internal class UpdateAccountProfileCommandHandler : IRequestHandler<UpdateAccountProfileCommand, ApiResponse<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IAccountProfileRepositoryAsync _accountProfileRepository;
        private readonly IUserRepositoryAsync _userRepository;

        public UpdateAccountProfileCommandHandler(IMapper mapper,
            IAccountProfileRepositoryAsync accountProfileRepository, IUserRepositoryAsync userRepository)
        {
            _mapper = mapper;
            _accountProfileRepository = accountProfileRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<Guid>> Handle(UpdateAccountProfileCommand request,
            CancellationToken cancellationToken)
        {
            // Kiểm tra người dùng có tồn tại không
            var result = await _userRepository.GetByIdAsync(request.UserId);
            if (result == null)
            {
                throw new ApiException("User not found.");
            }
            var accountProfile = _mapper.Map<AccountProfile>(request);
            accountProfile.LastModifiedBy = request.UserId.ToString();
            accountProfile.LastModified = DateTime.Now;
            await _accountProfileRepository.UpdateAsync(accountProfile);
            return new ApiResponse<Guid>(accountProfile.Id, "AccountProfile created successfully");
        }
    }
}