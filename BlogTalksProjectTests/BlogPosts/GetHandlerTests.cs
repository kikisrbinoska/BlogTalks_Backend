using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogTalks.Application.BlogPosts.Queries;
using BlogTalks.Domain.DTOs;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Reposotories;
using Moq;
using Xunit;

public class GetHandlerTests
{
    private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly GetHandler _handler;

    public GetHandlerTests()
    {
        _blogPostRepositoryMock = new Mock<IBlogPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _handler = new GetHandler(_blogPostRepositoryMock.Object, _userRepositoryMock.Object, _commentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedBlogPosts()
    {
        // Arrange
        var request = new GetRequest(1, 2, null, null);

        var blogPosts = new List<BlogPost>
        {
            new BlogPost { Id = 1, Title = "Post 1", CreatedBy = 1 },
            new BlogPost { Id = 2, Title = "Post 2", CreatedBy = 2 }
        };

        _blogPostRepositoryMock
            .Setup(r => r.GetPagedAsync(request.PageNumber, request.PageSize, request.SearchWord, request.Tag))
            .ReturnsAsync((blogPosts.Count, blogPosts));

        _userRepositoryMock
            .Setup(r => r.GetUsersByIds(It.IsAny<IEnumerable<int>>()))
            .Returns(blogPosts.Select(bp => new User { Id = bp.CreatedBy, Username = $"User{bp.CreatedBy}" }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
       
        Assert.Equal(2, result.Metadata.TotalCount);
        Assert.Equal(1, result.Metadata.PageNumber);
        Assert.Equal(2, result.Metadata.PageSize);
    }

   

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenNoPosts()
    {
        // Arrange
        var request = new GetRequest(1, 5, null, null);

        _blogPostRepositoryMock
            .Setup(r => r.GetPagedAsync(request.PageNumber, request.PageSize, request.SearchWord, request.Tag))
            .ReturnsAsync((0, new List<BlogPost>()));

        _userRepositoryMock
            .Setup(r => r.GetUsersByIds(It.IsAny<IEnumerable<int>>()))
            .Returns(new List<User>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Empty(result.BlogPosts);
        Assert.Equal(0, result.Metadata.TotalCount);
    }
}
