using BlogTalks.Application.User.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Reposotories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;
        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Registering a new user");
            var response = _mediator.Send(request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("User login attempt");
            var response = _mediator.Send(request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }

}

