using CodePulse.API.Models.Domain;
namespace CodePulse.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> UploadImageAsync(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAllImagesAsync();
    }
}
