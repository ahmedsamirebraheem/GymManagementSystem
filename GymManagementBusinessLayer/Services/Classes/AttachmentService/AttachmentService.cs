using GymManagementBusinessLayer.Services.Interfaces.AttachmentService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Classes.AttachmentService;

public class AttachmentService : IAttachmentService
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    private readonly long _maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
    private readonly IWebHostEnvironment _webHost;

    public  AttachmentService(IWebHostEnvironment webHost)
    {
        _webHost = webHost;
    }
    public async Task<string?> UploadAsync(string folderName, IFormFile file)
    {
        try
        {
            // 1. التحقق الأولي (حجم، امتداد، ونوع محتوى)
            if (file == null || string.IsNullOrEmpty(folderName) || file.Length == 0) return null;
            if (file.Length > _maxFileSizeInBytes) return null;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension) || !file.ContentType.StartsWith("image/"))
                return null;

            // 2. تجهيز المسارات
            var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}"; // استخدام Interpolation أسرع
            var filePath = Path.Combine(folderPath, fileName);

            // 3. النسخ (باستخدام using المختصرة)
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            // 4. إرجاع المسار بـ Slash في البداية
            return "/" + Path.Combine("images", folderName, fileName).Replace("\\", "/");
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to upload image to folder = {folderName}");
            // يفضل هنا تعمل Log للايرور عشان تعرف لو فيه مشكلة في السيرفر
            return null;
        }
    }
    public Task<bool> DeleteAsync(string folderName, string fileName)
    {
        try {
            if (file == null || string.IsNullOrEmpty(folderName) || file.Length == 0) return null;
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to delete image with name  = {fileName}");
            // يفضل هنا تعمل Log للايرور عشان تعرف لو فيه مشكلة في السيرفر
            return false;
        }
    }

}
