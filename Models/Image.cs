using System.ComponentModel.DataAnnotations;

namespace ImageUploadApp.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; }

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
    }
}