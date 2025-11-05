namespace CodePulse.API.Models.DTO
{
    public class UploadImageRequestDto
    {
        public IFormFile File { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
