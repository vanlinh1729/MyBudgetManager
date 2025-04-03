namespace MyBudgetManagement.Application.Helpers;

public static class ImageVerifier
{
    public static bool IsImageFile(string fileName)
    {
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        return allowedExtensions.Contains(Path.GetExtension(fileName).ToLower());
    }
}