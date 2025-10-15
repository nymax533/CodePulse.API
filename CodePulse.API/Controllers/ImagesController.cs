using Microsoft.AspNetCore.Mvc;
using CodePulse.API.Models.Domain;
namespace CodePulse.API.Controllers
{
    public class ImagesController : Controller
    {
        [HttpPost]

        public Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName,
            [FromForm] string title)
        {
            ValidateFile(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    Id = Guid.NewGuid(),
                    FileName = fileName,
                    FileType = file.ContentType,
                    FileExtension = Path.GetExtension(file.FileName),
                    Title = title,
                    Url = $"https://yourdomain.com/images/{fileName}{Path.GetExtension(file.FileName)}",
                    DateCreated = DateTime.UtcNow
                };
            }

        }
        private void ValidateFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
               ModelState.AddModelError("File", "Unsupported file type. Allowed types are: .jpg, .jpeg, .png, .gif");
            }
            if (file.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size exceeds the 10MB limit.");
            }
        }
    }
}
