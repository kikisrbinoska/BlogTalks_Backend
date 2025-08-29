using BlogTalks.Application.Comments.Comands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comment.Comands
{
    public class DeleteValidator : AbstractValidator<DeleteRequest>
    {
        public DeleteValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Comment ID cannot be empty.")
                .GreaterThan(0).WithMessage("Comment ID must be a positive number.");
        }
    }
}