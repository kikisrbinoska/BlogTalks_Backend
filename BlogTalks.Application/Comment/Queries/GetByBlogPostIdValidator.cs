using BlogTalks.Application.Comments.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comment.Queries
{
    public class GetByBlogPostIdValidator : AbstractValidator<GetByBlogPostIdRequest>
    {
        public GetByBlogPostIdValidator()
        {
            RuleFor(x => x.blogPostId)
                .NotEmpty().WithMessage("Blog post ID cannot be empty.")
                .GreaterThan(0).WithMessage("Blog post ID must be a positive integer.");
        }
    }
}
