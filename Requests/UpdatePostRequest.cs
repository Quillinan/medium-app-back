namespace medium_app_back.Requests
{
    public class UpdatePostRequest
    {
        public required string Title { get; set; }
        public required string Subtitle { get; set; }
        public required string Content { get; set; }
    }
}
