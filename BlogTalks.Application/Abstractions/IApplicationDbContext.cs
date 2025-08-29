using BlogTalks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Abstraction
{
    public interface IApplicationDbContext
    {
        public DbSet<Domain.Entities.BlogPost> BlogPosts { get; set; }
        public DbSet<Domain.Entities.Comment> Comments { get; set; }
    }
}
