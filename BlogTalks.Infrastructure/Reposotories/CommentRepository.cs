using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Reposotories;
using BlogTalks.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context,context.Comments )
    {
    }

    public IEnumerable<Comment> GetByBlogPostId(int blogPostId)
    {
        return _dbSet
            .Where(c => c.BlogPostId == blogPostId)
            .ToList();
    }

    public IEnumerable<Comment> GetCommentsByIds(IEnumerable<int> ids)
    {
        return
            _dbSet
            .Where(c => ids.Contains(c.Id))
            .ToList();
    }
}
