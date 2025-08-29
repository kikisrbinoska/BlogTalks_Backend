using BlogTalks.Application.Comments.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    namespace BlogTalks.Application.BlogPosts.Commands
    {
        public record AddResponse
        {
            public int Id { get; init; }
            public string Message { get; init; } = string.Empty;
            public DateTime CreatedAt { get; init; }
            public string CreatorName { get; init; } = string.Empty;
        }
    }
}
