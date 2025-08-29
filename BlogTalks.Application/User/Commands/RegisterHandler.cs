using BlogTalks.Domain.Reposotories;
using BlogTalks.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.User.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;

        public RegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;  
        }

        public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        { 

            if (_userRepository.GetByEmail(request.Email) != null)
            {
              return new RegisterResponse { Message = $"User with mail {request.Email} already exist" };
            }
            if (_userRepository.GetByUsername(request.Username) != null)
            {
                return new RegisterResponse { Message = $"User with username  {request.Username} already exists" };
            }
            var user = new Domain.Entities.User
            {
                Email = request.Email,
                Name = request.Name,
                Username = request.Username,
                Password = PasswordHasher.HashPassword(request.Password),
            };
            _userRepository.Add(user);
            return new RegisterResponse { Message = "Register successfull" };
        }
    }
}
