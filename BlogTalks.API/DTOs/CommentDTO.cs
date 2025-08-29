using BlogTalks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public required string Text { get; set; } 
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int BlogPostId { get; set; }
    }
}
