using Microsoft.AspNetCore.Components.Forms;

namespace RestaurantApp.Data.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IBrowserFile file, string folderPath);
        Task<bool> DeleteFileAsync(string filePath);
    }
}