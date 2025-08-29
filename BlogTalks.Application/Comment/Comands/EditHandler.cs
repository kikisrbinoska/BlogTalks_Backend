using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Reposotories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Comands
{
    public class EditHandler : IRequestHandler<EditRequest, EditResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditHandler(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<EditResponse> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            var comment =  _commentRepository.GetById(request.id);
            if (comment == null)
            {
                return null;
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return null;
            }
            comment.Text = request.Text;
            comment.CreatedAt = DateTime.UtcNow;
            

            _commentRepository.Update(comment);


            return Task.FromResult(new EditResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = comment.CreatedBy,
                BlogPostId = comment.BlogPostId
               
            });
        }
    }
}
