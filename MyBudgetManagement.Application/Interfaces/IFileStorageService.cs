namespace MyBudgetManagement.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string fileName);
}