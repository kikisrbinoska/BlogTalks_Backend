using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Exceptions.BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Reposotories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class DeleteBlogPostHandler : IRequestHandler<DeleteRequest, DeleteResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteBlogPostHandler(IBlogPostRepository blogPostRepository, IHttpContextAccessor httpContextAccessor)
        {
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<DeleteResponse> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.id);
            if (blogPost == null)
            {
                throw new BlogTalksException($"Blog post with Id {request.id} not found.", HttpStatusCode.NotFound);
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                throw new BlogTalksException("User not have correct access rights.", HttpStatusCode.Unauthorized);
            }
            if (blogPost.CreatedBy != currentUserId)
            {
                throw new BlogTalksException("You are not the author of this blog post.", HttpStatusCode.Forbidden);

            }
            _blogPostRepository.Delete(blogPost);
            return Task.FromResult(new DeleteResponse
            {
                Id = request.id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                Timestamp = blogPost.CreatedAt,
                CreatedBy = blogPost.CreatedBy
            });
        }
    }
}
