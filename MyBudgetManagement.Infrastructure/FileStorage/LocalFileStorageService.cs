using Microsoft.AspNetCore.Hosting;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadDirectory;

    public LocalFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
        _uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads");
        if (!Directory.Exists(_uploadDirectory))
        {
            Directory.CreateDirectory(_uploadDirectory);
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var filePath = Path.Combine(_uploadDirectory, fileName);
        var directoryPath = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath!);
        }

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        return fileName;
    }

    public Task DeleteFileAsync(string fileName)
    {
        var filePath = Path.Combine(_uploadDirectory, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }
}