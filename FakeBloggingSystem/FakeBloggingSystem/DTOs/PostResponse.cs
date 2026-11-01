namespace FakeBloggingSystem.DTOs
    {
    public record PostResponse
        {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public AuthorResponse? Author { get; set; }
        }
    }
