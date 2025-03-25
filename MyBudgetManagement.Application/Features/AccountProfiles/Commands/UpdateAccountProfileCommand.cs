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
    public Guid Id { get; set; }
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
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ApiException("User not found.");
            }

            // Lấy AccountProfile hiện tại
            var existingProfile = await _accountProfileRepository.GetAccountProfileByUserIdAsync(request.UserId);
            if (existingProfile == null)
            {
                throw new ApiException("AccountProfile not found.");
            }

            // Tạo một entity mới với dữ liệu từ request
            var updatedProfile = new AccountProfile
            {
                Id = existingProfile.Id,
                UserId = existingProfile.UserId,
                DateOfBirth = request.DateOfBirth,
                Avatar = request.Avatar,
                Address = request.Address,
                Gender = request.Gender,
                Currency = request.Currency,
                // Giữ lại các giá trị gốc của các trường audit
                CreatedBy = existingProfile.CreatedBy,
                Created = existingProfile.Created,
                LastModifiedBy = request.UserId.ToString(),
                LastModified = DateTime.Now
            };

            await _accountProfileRepository.UpdateAsync(updatedProfile);
            
            return new ApiResponse<Guid>(updatedProfile.Id, "AccountProfile updated successfully");
        }
    }
}