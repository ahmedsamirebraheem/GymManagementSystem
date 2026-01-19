using GymManagementBusinessLayer.Services.Interfaces.AttachmentService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// أضفنا الـ Namespace هنا لحل رسالة CA1050
namespace GymManagementBusinessLayer.Services.Classes.AttachmentService;

public class AttachmentService(IWebHostEnvironment webHost) : IAttachmentService
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    private readonly long _maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
    private readonly IWebHostEnvironment _webHost = webHost;

    public async Task<string?> UploadAsync(string folderName, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0 || string.IsNullOrEmpty(folderName))
                return null;

            if (file.Length > _maxFileSizeInBytes)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return null;

            // تحديد مسار المجلد: wwwroot/images/members
            var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            // نرجع المسار النسبي: images/members/guid.jpg
            return Path.Combine("images", folderName, fileName).Replace("\\", "/");
        }
        catch (Exception)
        {
            // يفضل عمل Logging هنا
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string folderName, string fileName)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName)) return false;

            // استخراج اسم الملف فقط لو كان المسار متخزن كامل
            var pureFileName = Path.GetFileName(fileName);
            var filePath = Path.Combine(_webHost.WebRootPath, "images", folderName, pureFileName);

            if (File.Exists(filePath))
            {
                // مسح الملف من الهارد ديسك
                File.Delete(filePath);
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}