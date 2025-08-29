using BlogTalks.Application.Comments.Queries;
using BlogTalks.Application.Contracts;
using BlogTalks.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public class GetResponse
    {
        public IEnumerable<BlogPostModel> BlogPosts { get; set; } = new List<BlogPostModel>();
        public Metadata Metadata { get; set; }
    }
}
