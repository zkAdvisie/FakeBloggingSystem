namespace FakeBloggingSystem.Models
    {
    public class Author
        {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public ICollection<Post> Posts { get; set; } 
        = new List<Post>();
        }
    }
