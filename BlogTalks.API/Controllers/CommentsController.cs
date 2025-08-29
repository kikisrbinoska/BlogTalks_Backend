using Azure.Core;
using BlogTalks.Application.Comments.Comands;
using BlogTalks.Application.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(IMediator mediator, ILogger<CommentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }



        // GET api/<CommentsController>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            _logger.LogInformation("Fetching all comments");
            var comments = await _mediator.Send(new GetRequest());
            return Ok(comments);
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}", Name = "GetCommentById")]
        [Authorize]
        public async Task<ActionResult> Get([FromRoute] GetCommentByIdRequest request)
        {
            _logger.LogInformation("Fetching comment with ID: {requestId}", request.id);
            var comment = await _mediator.Send(request);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }
        // GET api/<CommentsController>/5

        [HttpGet("/blogPosts/{blogPostId}/comments")]
        [Authorize]
        public async Task<ActionResult> Get([FromRoute] int blogPostId)
        {
            _logger.LogInformation("Fetching commnents for blog post with with ID: {blogPostId}", blogPostId);
            var comment = await _mediator.Send(new GetByBlogPostIdRequest(blogPostId));
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }


        // POST api/<CommentsController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] AddRequest request)
        {
            _logger.LogInformation("Adding a new comment");
            var commentToReturn = await _mediator.Send(request);
            if (commentToReturn == null)
            {
                return NotFound();
            }
            return Ok(commentToReturn);

        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody] EditRequest request)
        {
            _logger.LogInformation("Updating comment with ID: {id}", id);
            var commentToReturn = await _mediator.Send(new EditRequest(id,request.Text));
            if (commentToReturn == null)
            {
                return NotFound();
            }
            return NoContent();
        }


        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting comment with ID: {id}", id);
            var commentToReturn = await _mediator.Send(new DeleteRequest(id));
            if (commentToReturn == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }

   
}