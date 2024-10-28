namespace medium_app_back.Requests
{
    public class CreatePostRequest
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string AuthorId { get; set; }
    }
}
