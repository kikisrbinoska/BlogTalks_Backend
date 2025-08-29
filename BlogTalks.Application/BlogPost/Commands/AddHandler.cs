using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Reposotories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlogTalks.Application.BlogPosts.Commands.BlogTalks.Application.BlogPosts.Commands;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class AddBlogPostHandler : IRequestHandler<AddRequest, AddResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;   

        public AddBlogPostHandler(IBlogPostRepository blogPostRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<AddResponse> Handle(AddRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            int userIdValue = 0;
            if (!int.TryParse(userId, out userIdValue))
                userIdValue = 0;

            var blogPost = new Domain.Entities.BlogPost
            {
                Title = request.Title,
                Text = request.Text,
                CreatedBy = userIdValue,
                CreatedAt = DateTime.UtcNow,
                Tags = request.Tags,
            };

            _blogPostRepository.Add(blogPost);
            var creator = _userRepository.GetById(blogPost.CreatedBy)?.Name ?? "Unknown";
            var response = new AddResponse()
            {
                Id = blogPost.Id,
                CreatorName = creator,
                CreatedAt = blogPost.CreatedAt,
                Message = "Post created successfully"
            };

            return response;
        }
    }
}