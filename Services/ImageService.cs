using Microsoft.Extensions.Logging;

namespace ImageUploadApp.Services
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _logger;

        public ImageService(ILogger<ImageService> logger)
        {
            _logger = logger;
        }

        public Task<string> UploadImageAsync(IFormFile imageFile)
        {
            // Локальная реализация или заглушка
            throw new NotImplementedException("Use YandexImageService instead");
        }

        public Task<bool> DeleteImageAsync(string fileName)
        {
            throw new NotImplementedException("Use YandexImageService instead");
        }

        public void Dispose() { }
    }
}