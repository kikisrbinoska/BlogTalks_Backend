using BlogTalks.Application.Comments.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public record GetByIdResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Text { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }
}
