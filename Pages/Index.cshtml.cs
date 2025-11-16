using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageUploadApp.Data;
using ImageUploadApp.Models;
using ImageUploadApp.Services;

namespace ImageUploadApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public IndexModel(ILogger<IndexModel> logger,
                         ApplicationDbContext context,
                         IImageService imageService)
        {
            _logger = logger;
            _context = context;
            _imageService = imageService;
        }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();

        public async Task OnGetAsync()
        {
            Images = await _context.Images
                .OrderByDescending(i => i.UploadDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Please select an image file.");
                await OnGetAsync();
                return Page();
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(ImageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("ImageFile", "Only image files (JPEG, PNG, GIF) are allowed.");
                await OnGetAsync();
                return Page();
            }

            if (ImageFile.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "File size must be less than 10MB.");
                await OnGetAsync();
                return Page();
            }

            try
            {
                var fileUrl = await _imageService.UploadImageAsync(ImageFile);

                var image = new Image
                {
                    FileName = ImageFile.FileName,
                    FilePath = fileUrl,
                    UploadDate = DateTime.UtcNow,
                    FileSize = ImageFile.Length,
                    ContentType = ImageFile.ContentType ?? "application/octet-stream"
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Image uploaded successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                ModelState.AddModelError("", "Error uploading image. Please try again.");
                await OnGetAsync();
                return Page();
            }
        }
    }
}