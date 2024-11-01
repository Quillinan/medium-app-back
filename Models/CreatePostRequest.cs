namespace medium_app_back.Models
{
    public class CreatePostRequest
    {
        public required string Title { get; set; }
        public required string Subtitle { get; set; }
        public required string Content { get; set; }
        public required IFormFile CoverImageData { get; set; }
        public required string AuthorId { get; set; }
        public required string AuthorName { get; set; }
    }
}
