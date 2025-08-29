using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.User.Commands
{
    public record RegisterRequest(string Email,string Username,string Name, string Password) : IRequest<RegisterResponse>
    {
    }
}
