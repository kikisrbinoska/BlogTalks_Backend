using BlogTalks.Application.Abstractions;
using BlogTalks.Application.Comments.Comands;
using BlogTalks.Domain.Reposotories;
using BlogTalks.EmailSenderApi.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace BlogTalks.Application.Comment.Comands;

public class AddHandler : IRequestHandler<AddRequest, AddResponse>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IFeatureManager _featureManager;
    private readonly ILogger<AddHandler> _logger;

    public AddHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IServiceProvider serviceProvider, IFeatureManager featureManager, ILogger<AddHandler> logger)
    {
        _commentRepository = commentRepository;
        _blogPostRepository = blogPostRepository;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _serviceProvider = serviceProvider;
        _featureManager = featureManager;
        _logger = logger;
    }

    public async Task<AddResponse> Handle(AddRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
        var userIdValue = 0;
        if (int.TryParse(userId, out int parsedUserId))
        {
            userIdValue = parsedUserId;
        }
        var blogPost = _blogPostRepository.GetById(request.BlogPostId);
        if (blogPost == null)
        {
            return null;
        }

        var comment = new Domain.Entities.Comment
        {
            Text = request.Text,
            CreatedBy = userIdValue,
            CreatedAt = DateTime.UtcNow,
            BlogPostId = request.BlogPostId,
            BlogPost = blogPost
        };

        _commentRepository.Add(comment);

        var blogpostCreator = _userRepository.GetById(blogPost.CreatedBy);
        var commentCreator = _userRepository.GetById(userIdValue);

        await SendEmail(commentCreator, blogpostCreator, blogPost);

        return new AddResponse(comment.Id);
    }

    private async Task SendEmail(Domain.Entities.User? commentCreator, Domain.Entities.User? blogpostCreator, Domain.Entities.BlogPost blogPost)
    {
        if (commentCreator == null || blogpostCreator == null)
        {
            _logger.LogError("Comment creator or blog post creator is null. Email cannot be sent.");
            return;
        }

        var dto = new EmailDTO
        {
            From = commentCreator.Email,
            To = blogpostCreator.Email,
            Subject = "New Comment Added",
            Body = $"A new comment has been added to the blog post '{blogPost.Title}' by user {commentCreator.Name}."
        };

        string[] featureFlags = ["EmailSenderHTTP", "EmailSenderRabbitMQ"];
        foreach (var featureFlag in featureFlags)
        {
            if (!await _featureManager.IsEnabledAsync(featureFlag)) continue;
            var service = _serviceProvider.GetRequiredKeyedService<IMessagingService>(
                featureFlag == "EmailSenderHTTP" ? "MessagingServiceHttp" : "MessagingServiceRabbitMQ");
            await service.Send(dto);
            return;
        }

        _logger.LogError("Email service is not available");
    }
}