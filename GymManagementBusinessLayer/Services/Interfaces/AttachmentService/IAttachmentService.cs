using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Interfaces.AttachmentService;

public interface IAttachmentService 
{
    Task<string?> UploadAsync(string folderName, IFormFile fileName);
    Task<bool> DeleteAsync(string folderName, string fileName);
}
