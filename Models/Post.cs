namespace medium_app_back.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Subtitle { get; set; }
        public required string Content { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public required string AuthorId { get; set; }
        public required string AuthorName { get; set; }

    }

}
