using BlogTalks.Application.Comments.Comands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comment
{
    public class EditValidator : AbstractValidator<EditRequest> 
    {
        public EditValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Comment ID is required.");
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Comment text cannot be empty.");
              
        }
    }
}
