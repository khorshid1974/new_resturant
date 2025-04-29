using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RestaurantApp.Data.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IBrowserFile file, string folderPath)
        {
            try
            {
                // Create a unique file name to prevent overwriting existing files
                string fileName = $"{Guid.NewGuid()}_{file.Name}";
                
                // Ensure the directory exists
                string directoryPath = Path.Combine(_environment.WebRootPath, folderPath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create the full file path
                string filePath = Path.Combine(directoryPath, fileName);
                
                // Create a file stream and copy the uploaded file to it
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.OpenReadStream(maxAllowedSize: 10485760).CopyToAsync(fileStream); // 10MB max
                }

                // Return the relative path to be stored in the database
                return Path.Combine(folderPath, fileName).Replace("\\", "/");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return Task.FromResult(false);

                string fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
                
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return Task.FromResult(true);
                }
                
                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}