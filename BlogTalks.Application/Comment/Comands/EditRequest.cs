using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Comands
{
    public record EditRequest([property: JsonIgnore]int id,string Text) : IRequest<EditResponse>;

}
