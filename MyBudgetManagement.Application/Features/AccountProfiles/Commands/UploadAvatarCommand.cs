using MediatR;
using Microsoft.AspNetCore.Http;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.AccountProfiles.Commands;

public class UploadAvatarCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    public IFormFile AvatarFile { get; set; }

    internal class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, ApiResponse<string>>
    {
        private readonly IAccountProfileRepositoryAsync _accountProfileRepository;
        private readonly IFileStorageService _fileStorageService;

        public UploadAvatarCommandHandler(
            IAccountProfileRepositoryAsync accountProfileRepository,
            IFileStorageService fileStorageService)
        {
            _accountProfileRepository = accountProfileRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<ApiResponse<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
        {
            var profile = await _accountProfileRepository.GetAccountProfileByUserIdAsync(request.UserId);
            if (profile == null)
            {
                throw new ApiException("AccountProfile not found");
            }

            // Validate file
            if (request.AvatarFile == null || request.AvatarFile.Length == 0)
            {
                throw new ApiException("No file uploaded");
            }

            if (!IsImageFile(request.AvatarFile.FileName))
            {
                throw new ApiException("Only image files are allowed");
            }

            if (request.AvatarFile.Length > 5 * 1024 * 1024) // 5MB limit
            {
                throw new ApiException("File size cannot exceed 5MB");
            }

            // Delete old avatar if exists
            if (!string.IsNullOrEmpty(profile.Avatar))
            {
                await _fileStorageService.DeleteFileAsync(profile.Avatar);
            }

            // Upload to Cloudinary
            using var stream = new MemoryStream();
            await request.AvatarFile.CopyToAsync(stream, cancellationToken);
            stream.Position = 0; // Reset stream position before reading
            
            var imageUrl = await _fileStorageService.UploadFileAsync(stream, request.AvatarFile.FileName);

            // Update profile with Cloudinary URL
            profile.Avatar = imageUrl;
            profile.LastModified = DateTime.Now;
            profile.LastModifiedBy = request.UserId.ToString();
            await _accountProfileRepository.UpdateAsync(profile);

            return new ApiResponse<string>(imageUrl, "Avatar uploaded successfully");
        }

        private bool IsImageFile(string fileName)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(Path.GetExtension(fileName).ToLower());
        }
    }
}