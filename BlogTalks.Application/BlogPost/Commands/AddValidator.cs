using BlogTalks.Application.BlogPosts.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class AddValidator : AbstractValidator<AddRequest>
    {
        public AddValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.");

        }
    }
}
