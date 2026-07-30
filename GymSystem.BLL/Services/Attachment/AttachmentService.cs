using GymManagementSystem.BLL.Services.Attachment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GymSystem.BLL.Services.Attachment
{
    public class AttachmentService : IAttachmentService
    {

        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly ILogger<AttachmentService> _logger;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly IWebHostEnvironment _env;
        public AttachmentService(ILogger<AttachmentService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0) return null;
            if (fileStream.Length > _maxFileSize)
            {
                _logger.LogWarning($"Rejected upload: file too large ({fileStream.Length} bytes).");
                return null;
            }

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            {
                _logger.LogWarning($"Rejected upload: extension {extension} not allowed.");
                return null;
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);
            Directory.CreateDirectory(uploadsFolder);

            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, storedFileName);

            try
            {
                using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                await fileStream.CopyToAsync(fs, ct);
                return storedFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName}.", fileName);
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

            try
            {
                var fullPath = Path.Combine(_env.WebRootPath, folderName, fileName);

                if (!File.Exists(fullPath)) return false;
                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete attachment {File}.", fileName);
                return false;
            }
        }

        public (Stream Stream, string ContentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return null;

            var fullPath = Path.Combine(_env.WebRootPath, folderName, fileName);

            if (!File.Exists(fullPath)) return null;

            var contentType = Path.GetExtension(fullPath).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, contentType);
        }

    }
}