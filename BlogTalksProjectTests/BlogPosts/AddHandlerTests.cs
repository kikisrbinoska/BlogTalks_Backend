//using BlogTalks.Application.BlogPosts.Commands;
//using BlogTalks.Domain.Entities;
//using BlogTalks.Domain.Reposotories;
//using Microsoft.AspNetCore.Http;
//using Moq;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Xunit;

//namespace BlogTalksProjectTests.BlogPosts
//{
//    public class AddHandlerTests
//    {
//        private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
//        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
//        private readonly AddBlogPostHandler _handler;

//        public AddHandlerTests()
//        {
//            _blogPostRepositoryMock = new Mock<IBlogPostRepository>();
//            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
//            _handler = new AddBlogPostHandler(_blogPostRepositoryMock.Object, _httpContextAccessorMock.Object);
//        }

//        [Fact]
//        public async Task Handle_ValidRequest_ReturnsCreateResponse()
//        {
//            var userId = "42";
//            var claims = new[] { new Claim("userId", userId) };   
//            var identity = new ClaimsIdentity(claims, "TestAuthType");
//            var principal = new ClaimsPrincipal(identity);

//            var httpContextMock = new Mock<HttpContext>();
//            httpContextMock.Setup(c => c.User).Returns(principal);
//            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

//            var request = new AddRequest("Test Title", "Test Content", new List<string> { "tag1", "tag2" });

//            _blogPostRepositoryMock
//                .Setup(r => r.Add(It.IsAny<BlogPost>()))
//                .Callback<BlogPost>(post =>
//                {
//                    post.Id = 123;
//                });

//            var result = await _handler.Handle(request, default);

//            Assert.NotNull(result);
//            Assert.Equal(123, result.Id);

//            _blogPostRepositoryMock.Verify(r => r.Add(It.Is<BlogPost>(b =>
//                b.Title == request.Title &&
//                b.Text == request.Text &&
//                b.CreatedBy == int.Parse(userId) &&
//                b.Tags.SequenceEqual(request.Tags)   
//            )), Times.Once);
//        }
//    }
//}
