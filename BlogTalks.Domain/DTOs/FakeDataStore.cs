using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.DTOs
{
    public class FakeDataStore
    {
        private static List<CommentDTO> _comments;
        private static List<BlogPostDto> comments;
        public FakeDataStore()
        {
            _comments = new List<CommentDTO>
            {
                new CommentDTO { Id = 1, Text = "This is the first comment", CreatedAt = DateTime.Now, CreatedBy = 1, BlogPostId = 1 },
                new CommentDTO { Id = 2, Text = "This is the second comment", CreatedAt = DateTime.Now, CreatedBy = 2, BlogPostId =  1}
            };
            comments = new List<BlogPostDto>
            {
                new BlogPostDto { Id = 1, Title = "First Post", Text = "This is the first post", CreatedBy = 1, Timestamp = DateTime.Now, Tags = new List<string> { "tag1", "tag2" }, Comments = _comments },
                new BlogPostDto { Id = 2, Title = "Second Post", Text = "This is the second post", CreatedBy = 2, Timestamp = DateTime.Now, Tags = new List<string> { "tag3" }, Comments = _comments }
            };
        }
        public async Task AddComment (CommentDTO comment)
        {
            _comments.Add(comment);
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<CommentDTO>> GetAllComments()
        {
            return await Task.FromResult(_comments);
        }
        public async Task<CommentDTO?> GetCommentById(int id) => await Task.FromResult(_comments.SingleOrDefault(p => p.Id == id));
       
        public async Task UpdateComment(int id, CommentDTO comment)
        {
           var existingComment = _comments.SingleOrDefault(x => x.Id == id);
            if(existingComment != null)
            {
                existingComment.Text = comment.Text;
                existingComment.CreatedAt = DateTime.Now;
                existingComment.CreatedBy = comment.CreatedBy;
                existingComment.BlogPostId = comment.BlogPostId;
                existingComment.Id = comment.Id;
            }
            await Task.CompletedTask;
        }
        public async Task DeleteComment(int id)
        {
            var existingComment = _comments.SingleOrDefault(x => x.Id == id);
            if (existingComment != null)
            {
                _comments.Remove(existingComment);
            }
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<BlogPostDto>> GetAllBlogPosts()
        {
            return await Task.FromResult(comments);
        }
        public async Task<BlogPostDto?> GetBlogPostById(int id)
        {
            return await Task.FromResult(comments.SingleOrDefault(p => p.Id == id));
        }
        public async Task<IEnumerable<CommentDTO>> GetByBlogPostId(int blogPostId)
        {
           
            return await Task.FromResult(_comments.Where(p => p.BlogPostId == blogPostId));
            
        }
        public async Task<BlogPostDto> AddBlogPost(BlogPostDto blogPost)
        {
            comments.Add(blogPost);
            await Task.CompletedTask;
            return blogPost;
        }
        public async Task UpdateBlogPost(int id,BlogPostDto dto)
        {
            var existingPost = comments.SingleOrDefault(x => x.Id == id);
            if(existingPost != null)
            {
                existingPost.Title = dto.Title;
                existingPost.Text = dto.Text;
                existingPost.CreatedBy = dto.CreatedBy;
                existingPost.Timestamp = DateTime.Now;
                existingPost.Tags = dto.Tags;
                existingPost.Comments = dto.Comments;
            }
            await Task.CompletedTask;
        }
        public async Task DeleteBlogPost(int id)
        {
           var blogPost = comments.SingleOrDefault(x => x.Id == id);
            if(blogPost != null)
            {
                comments.Remove(blogPost);
            }
            await Task.CompletedTask;
        }

    }
}
