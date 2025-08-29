using BlogTalks.Application.BlogPosts.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class EditValidator : AbstractValidator<EditRequest>
    {
        public EditValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Blog post ID is required.")
                .GreaterThan(0).WithMessage("Blog post ID must be greater than 0.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
}
