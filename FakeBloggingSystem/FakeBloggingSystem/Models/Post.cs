namespace FakeBloggingSystem.Models
    {
    public class Post
        {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public  string Content { get; set; } = string.Empty;
        public Author Author { get; set; } = null!;
        }
    }
