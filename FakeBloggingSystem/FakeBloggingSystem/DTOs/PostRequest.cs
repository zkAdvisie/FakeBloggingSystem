using System.ComponentModel.DataAnnotations;

namespace FakeBloggingSystem.DTOs
    {
    public record PostRequest
        {
        [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be greater than 0.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;
        }
    }

