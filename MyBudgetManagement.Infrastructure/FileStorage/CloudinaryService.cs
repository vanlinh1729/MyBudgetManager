using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Infrastructure.FileStorage;

public class CloudinaryService : IFileStorageService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryService> _logger;

    public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
    {
        _logger = logger;

        try 
        {
            var cloudinarySection = configuration.GetSection("Cloudinary");
            _logger.LogInformation("Cloudinary Section exists: {exists}", cloudinarySection.Exists());
            
            var cloudName = cloudinarySection["CloudName"];
            var apiKey = cloudinarySection["ApiKey"];
            var apiSecret = cloudinarySection["ApiSecret"];

            _logger.LogInformation("Cloudinary Config - CloudName: {cloudName}, ApiKey: {apiKeyExists}, ApiSecret: {apiSecretExists}", 
                cloudName,
                !string.IsNullOrEmpty(apiKey),
                !string.IsNullOrEmpty(apiSecret));

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new ApiException("Cloudinary configuration is missing or invalid");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing Cloudinary service");
            throw new ApiException($"Failed to initialize Cloudinary: {ex.Message}");
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null)
        {
            throw new ApiException("File stream is null");
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ApiException("File name is empty");
        }

        try
        {
            _logger.LogInformation("Attempting to upload file: {fileName}", fileName);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "avatars",
                Transformation = new Transformation()
                    .Width(500)
                    .Height(500)
                    .Crop("fill")
                    .Gravity("face")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            
            if (uploadResult.Error != null)
            {
                _logger.LogError("Cloudinary upload error: {error}", uploadResult.Error.Message);
                throw new ApiException($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }

            _logger.LogInformation("File uploaded successfully: {url}", uploadResult.SecureUrl);
            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {fileName}", fileName);
            throw new ApiException($"File upload failed: {ex.Message}");
        }
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
        {
            _logger.LogInformation("No file URL provided for deletion");
            return;
        }

        try
        {
            _logger.LogInformation("Attempting to delete file: {fileUrl}", fileUrl);

            var uri = new Uri(fileUrl);
            var pathSegments = uri.Segments;
            var fileName = pathSegments[pathSegments.Length - 1];
            var publicId = $"avatars/{Path.GetFileNameWithoutExtension(fileName)}";

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                _logger.LogError("Cloudinary delete error: {error}", result.Error.Message);
                throw new ApiException($"Cloudinary delete failed: {result.Error.Message}");
            }

            _logger.LogInformation("File deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {fileUrl}", fileUrl);
            throw new ApiException($"File deletion failed: {ex.Message}");
        }
    }
}