using BlogTalks.Application.BlogPosts.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Queries
{
    public class GetByIdValidator : AbstractValidator<GetByIdRequest>
    {
       public GetByIdValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Blog post ID cannot be empty.")
                .GreaterThan(0).WithMessage("Blog post ID must be a positive number.");
        }
    }
}
