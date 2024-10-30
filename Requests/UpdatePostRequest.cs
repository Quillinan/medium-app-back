namespace medium_app_back.Requests
{
    public class UpdatePostRequest
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Content { get; set; }
        public string? CoverImage { get; set; }
    }
}
