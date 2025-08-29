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
    public class GetHandler : IRequestHandler<GetRequest, IEnumerable<GetResponse>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Task<IEnumerable<GetResponse>> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            var comments = _commentRepository.GetAll();

            var response = comments.Select(c => new GetResponse
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                BlogPostId = c.BlogPostId
            });

            return Task.FromResult(response);
        }
    }

}
