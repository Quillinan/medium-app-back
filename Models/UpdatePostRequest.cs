namespace medium_app_back.Models
{
    public class UpdatePostRequest
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Content { get; set; }
        public IFormFile? CoverImageData { get; set; }
    }
}
