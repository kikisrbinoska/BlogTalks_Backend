using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Contracts
{
    public class BlogPostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string CreatorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}
