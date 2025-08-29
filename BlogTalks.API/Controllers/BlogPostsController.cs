using Azure.Core;
using BlogTalks.Application.BlogPosts.Commands;
using BlogTalks.Application.BlogPosts.Queries;
using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Entities;
using BlogTalks.Infrastructure.Atuhenticatin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(IMediator mediator, ILogger<BlogPostsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }



        // GET: api/<BlogPostsController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Get([FromQuery] GetRequest request)
        {
            _logger.LogInformation("Fetching all blog posts");
            var blogPosts = await _mediator.Send(request);
            return Ok(blogPosts);
        }

        // GET api/<BlogPostsController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> Get([FromRoute] GetByIdRequest request )
        {
            _logger.LogInformation("Fetching blog post with ID: {requestId}", request.id);
            var blogPost = await _mediator.Send(request);
            return Ok(blogPost);
                       
        }


        // POST api/<BlogPostsController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Add([FromBody] AddRequest request)
        {
            _logger.LogInformation("Adding a new blog post");
            var blogPost = await _mediator.Send(request);

            return Ok(blogPost);
        }



        // PUT api/<BlogPostsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put([FromRoute]int id, [FromBody] EditRequest request)
        {
            _logger.LogInformation("Fetching blog post with ID: {id}", id);  
            var blogPosts = await _mediator.Send(new EditRequest(id, request.Title, request.Text, request.Tags));
            return NoContent();
        }

        // DELETE api/<BlogPostsController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting blog post with ID: {id}",id);
            var blogPosts = await _mediator.Send(new DeleteRequest(id));   
            return NoContent();
        }
    }

  
}
