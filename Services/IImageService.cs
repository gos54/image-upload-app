using System;

namespace ImageUploadApp.Services
{
    public interface IImageService : IDisposable
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
        Task<bool> DeleteImageAsync(string fileName);
    }
}