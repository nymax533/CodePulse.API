using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepositry : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;
        public ImageRepositry(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }
        public async Task<BlogImage> UploadImageAsync(IFormFile file, BlogImage blogImage)
        {
            var locationPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using (var stream = new FileStream(locationPath, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var imageUrl =
                $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{httpRequest.Host}{httpRequest.PathBase}/Images/" +
                $"{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = imageUrl;

            await _dbContext.BlogImages.AddAsync(blogImage);
            await _dbContext.SaveChangesAsync();
            return blogImage;
        }
    
        public async Task<IEnumerable<BlogImage>> GetAllImagesAsync()
        {
            return await _dbContext.BlogImages.ToListAsync();
        }
    }
}
