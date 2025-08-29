using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Queries
{
    public record GetByBlogPostIdRequest(int blogPostId) : IRequest<IEnumerable<GetByBlogPostIdResponse>>;

}
