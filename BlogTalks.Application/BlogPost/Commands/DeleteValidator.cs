using BlogTalks.Application.BlogPosts.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class DeleteValidator : AbstractValidator<DeleteRequest>
    {
        public DeleteValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Blog post ID cannot be empty.")
                .GreaterThan(0).WithMessage("Blog post ID must be greater than zero.");
        }
    }
}
