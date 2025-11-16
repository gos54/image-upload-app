using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;

namespace ImageUploadApp.Services
{
    public class YandexImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<YandexImageService> _logger;

        public YandexImageService(IConfiguration configuration, ILogger<YandexImageService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var storageConfig = _configuration.GetSection("YandexCloud:ObjectStorage");
            var accessKey = storageConfig["AccessKey"];
            var secretKey = storageConfig["SecretKey"];

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("S3 credentials are not configured properly");
            }

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);

            var s3Config = new AmazonS3Config
            {
                ServiceURL = storageConfig["ServiceUrl"],
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(awsCredentials, s3Config);
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var bucketName = _configuration["YandexCloud:ObjectStorage:BucketName"];

            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException("Bucket name is not configured");
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";

            try
            {
                using var stream = imageFile.OpenReadStream();
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = imageFile.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await _s3Client.PutObjectAsync(putRequest);
                _logger.LogInformation($"Image uploaded successfully: {fileName}");

                return $"https://{bucketName}.storage.yandexcloud.net/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading image to S3: {fileName}");
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(string fileName)
        {
            try
            {
                var bucketName = _configuration["YandexCloud:ObjectStorage:BucketName"];
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
                _logger.LogInformation($"Image deleted successfully: {fileName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting image from S3: {fileName}");
                return false;
            }
        }

        public void Dispose()
        {
            _s3Client?.Dispose();
        }
    }
}