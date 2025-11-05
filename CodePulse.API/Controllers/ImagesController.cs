
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using CodePulse.API.Models.DTO;

namespace CodePulse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestDto request)
        {
            if (request?.File == null)
            {
                ModelState.AddModelError("File", "File is required.");
                return BadRequest(ModelState);
            }

            ValidateFile(request.File);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blogImage = new BlogImage
            {
                Id = Guid.NewGuid(),
                FileName = request.FileName,
                FileType = request.File.ContentType,
                FileExtension = Path.GetExtension(request.File.FileName),
                Title = request.Title,
                DateCreated = DateTime.UtcNow
            };

            blogImage = await _imageRepository.UploadImageAsync(request.File, blogImage);

            var response = new BlogImageDto
            {
                Id = blogImage.Id,
                FileName = blogImage.FileName,
                FileType = blogImage.FileType,
                FileExtension = blogImage.FileExtension,
                Title = blogImage.Title,
                Url = blogImage.Url,
                DateCreated = blogImage.DateCreated
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>  GetAllImages()
        {
            var images = await _imageRepository.GetAllImagesAsync();
            var response = images.Select(blogImage => new BlogImageDto
            {
                Id = blogImage.Id,
                FileName = blogImage.FileName,
                FileType = blogImage.FileType,
                FileExtension = blogImage.FileExtension,
                Title = blogImage.Title,
                Url = blogImage.Url,
                DateCreated = blogImage.DateCreated
            });
            return Ok(response);
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
