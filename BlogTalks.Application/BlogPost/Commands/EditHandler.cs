using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Application.Comments.Queries;
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
    public class EditHandler : IRequestHandler<EditRequest, EditBlogPostResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EditHandler(IBlogPostRepository blogPostRepository, IHttpContextAccessor httpContextAccessor)
        {
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<EditBlogPostResponse> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            var blogPost =_blogPostRepository.GetById(request.Id);

            if (blogPost == null)
            {
                throw new BlogTalksException($"Blog post with Id {request.Id} not found.", HttpStatusCode.NotFound);
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
            blogPost.Title = request.Title;
            blogPost.Text = request.Text;
            blogPost.CreatedAt = DateTime.UtcNow;
            blogPost.Tags = request.Tags;   


            _blogPostRepository.Update(blogPost);


            return Task.FromResult(new EditBlogPostResponse
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                CreatedBy = blogPost.CreatedBy,
                Timestamp = DateTime.UtcNow,
                Tags = blogPost.Tags,
            });
        }
    }
}
