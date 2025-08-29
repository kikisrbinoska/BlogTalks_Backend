using BlogTalks.Application.BlogPosts.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comment.Queries
{
    public class GetByIdValidator : AbstractValidator<GetByIdRequest>
    {
        public GetByIdValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
