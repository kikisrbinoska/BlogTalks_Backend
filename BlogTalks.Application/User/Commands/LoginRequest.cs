using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.User.Commands
{
    public record LoginRequest(string Email, string Username,string Password) : IRequest<LoginResponse>
    {
    }
}
