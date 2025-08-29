using BlogTalks.Application.Comments.Comands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comment.Comands
{
    public class EditValidator : AbstractValidator<EditRequest>
    {
        public EditValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Id is required.");
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Text is required."); 
        }
    }
}