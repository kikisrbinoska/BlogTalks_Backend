
namespace BlogTalks.Domain.DTOs
{
    public class BlogPostDto
    {      
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Text { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();

    }
}
