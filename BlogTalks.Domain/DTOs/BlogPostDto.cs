using BlogTalks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.DTOs
{
    public class BlogPostDto
    {      
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Text { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public List<string> Tags { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();

    }
}
