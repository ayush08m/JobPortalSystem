using Microsoft.AspNetCore.Http;

namespace JobPortalSystem.Helpers
{
    public static class FileHelper
    {
        private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public static bool IsValidResumeFile(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (file == null || file.Length == 0)
            {
                errorMessage = "Please select a file to upload.";
                return false;
            }

            if (file.Length > MaxFileSize)
            {
                errorMessage = $"File size must not exceed {MaxFileSize / 1024 / 1024}MB.";
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                errorMessage = $"Only {string.Join(", ", AllowedExtensions)} files are allowed.";
                return false;
            }

            return true;
        }

        public static async Task<string> SaveResumeAsync(IFormFile file, string uploadsFolder, string userId)
        {
            try
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{userId}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving file: {ex.Message}");
            }
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
