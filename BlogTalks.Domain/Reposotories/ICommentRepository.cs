using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Shared;
using BlogTalks.Infrastructure.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Reposotories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetByBlogPostId(int blogPostId);
        public IEnumerable<Comment> GetCommentsByIds(IEnumerable<int> ids);
    }
}
