using BlogTalks.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        //Navigation 
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = new BlogPost();
    }
}
