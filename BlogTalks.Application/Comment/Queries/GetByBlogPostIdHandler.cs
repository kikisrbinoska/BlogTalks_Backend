using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Reposotories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Queries
{
    public class GetByBlogPostIdHandler : IRequestHandler<GetByBlogPostIdRequest, IEnumerable<GetByBlogPostIdResponse>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserRepository _userRepository;


        public GetByBlogPostIdHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<GetByBlogPostIdResponse>> Handle(GetByBlogPostIdRequest request, CancellationToken cancellationToken)
        {  
            var blogPost = _blogPostRepository.GetById(request.blogPostId);

            var list = _commentRepository.GetByBlogPostId(request.blogPostId);

            
            var getAllResponseList = new List<GetByBlogPostIdResponse>();
            foreach (var item in list)
            {
                var response = new GetByBlogPostIdResponse
                {
                    Id = item.Id,
                    Text = item.Text, 
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    CreatorName = _userRepository.GetById(item.CreatedBy)?.Username ?? "Unknown",
                };
                getAllResponseList.Add(response);
            }

            return getAllResponseList;
        }
    }
}
