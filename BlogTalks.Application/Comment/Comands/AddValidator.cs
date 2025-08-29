using BlogTalks.Application.Comments.Comands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comment.Comands
{
    public class AddValidator : AbstractValidator<AddRequest>
    {
        public AddValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Comment text cannot be empty.");   
            RuleFor(x => x.BlogPostId)
                .GreaterThan(0).WithMessage("Blog post ID must be greater than zero.");
        }
    }
}