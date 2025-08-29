using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Reposotories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Queries
{
    public class GetByIdHandler : IRequestHandler<GetCommentByIdRequest, GetCommentByIdResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public GetByIdHandler(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public Task<GetCommentByIdResponse> Handle(GetCommentByIdRequest request, CancellationToken cancellationToken)
        {
            var comment = _commentRepository.GetById(request.id);
            if (comment == null)
            {
                return null;
            }

            return Task.FromResult(new GetCommentByIdResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                CreatedBy = comment.CreatedBy,
                BlogPostId = comment.BlogPostId,
                CreatorName = _userRepository.GetById(comment.CreatedBy)?.Username ?? "Unknown",
            });
        }

    }
}
