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
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        public Task<(int count, List<BlogPost> list)> GetPagedAsync(int? pageNumber, int? pageSize, string? searchWord, string? tag);
        public int GetTotalNumber();


    }
}
