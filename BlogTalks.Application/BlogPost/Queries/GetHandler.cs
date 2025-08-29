using BlogTalks.Application.Comments.Queries;
using BlogTalks.Application.Contracts;
using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Reposotories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPosts.Queries
{
    public class GetHandler : IRequestHandler<GetRequest, GetResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;

        public GetHandler(IBlogPostRepository blogPostRepository, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public async Task<GetResponse> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber ?? 1;
            var pageSize = request.PageSize ?? 9;

            var (totalCount, blogPosts) = await _blogPostRepository.GetPagedAsync(
                pageNumber,
                pageSize,
                request.SearchWord,
                request.Tag
            );

            var userIds = blogPosts.Select(bp => bp.CreatedBy).Distinct().ToList();
            var users = _userRepository.GetUsersByIds(userIds);

            var userDict = users.ToDictionary(u => u.Id, u => u.Username);
            var blogPostModels = blogPosts.Select(bp => new BlogPostModel
            {
                Id = bp.Id,
                Title = bp.Title,
                Text = bp.Text,
                Tags = bp.Tags,
                CreatorName = userDict.GetValueOrDefault(bp.CreatedBy, string.Empty),
                CreatedAt = bp.CreatedAt,
                Comments = _commentRepository.GetByBlogPostId(bp.Id).Select(c => new CommentModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt
                }).ToList(),
            }).ToList();

            var response = new GetResponse
            {
                BlogPosts = blogPostModels,
                Metadata = new Metadata
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            };

            return response;
        }
    }
}
