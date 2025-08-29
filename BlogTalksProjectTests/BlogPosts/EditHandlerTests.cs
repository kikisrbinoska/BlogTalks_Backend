using BlogTalks.Application.BlogPosts.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Exceptions.BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Reposotories;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlogTalksProjectTests.BlogPosts
{
    public class EditHandlerTests
    {
        private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly EditHandler _handler;

        public EditHandlerTests()
        {
            _blogPostRepositoryMock = new Mock<IBlogPostRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _handler = new EditHandler(_blogPostRepositoryMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Handle_BlogPostNotExist_ThrowsNotFound()
        {
            
            var request = new EditRequest(1, "Title", "Text", new List<string> { "Tag1" });
            _blogPostRepositoryMock.Setup(r => r.GetById(1)).Returns((BlogPost)null);

            var ex = await Assert.ThrowsAsync<BlogTalksException>(
                () => _handler.Handle(request, CancellationToken.None));

            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }

        [Fact]
        public async Task Handle_InvalidUserIdClaim_ThrowsUnauthorized()
        {
            var blogPost = new BlogPost { Id = 1, CreatedBy = 5 };
            _blogPostRepositoryMock.Setup(r => r.GetById(1)).Returns(blogPost);

            var httpContextMock = new Mock<HttpContext>();
            var claims = new[] { new Claim("userId", "invalid-id") };  
            httpContextMock.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(claims)));
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var request = new EditRequest(1, "Title", "Text", new List<string>());

            var ex = await Assert.ThrowsAsync<BlogTalksException>(
                () => _handler.Handle(request, CancellationToken.None));

            Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
        }

        [Fact]
        public async Task Handle_UserNotAuthor_ThrowsForbidden()
        {
            var blogPost = new BlogPost { Id = 1, CreatedBy = 99 };  
            _blogPostRepositoryMock.Setup(r => r.GetById(1)).Returns(blogPost);

            var httpContextMock = new Mock<HttpContext>();
            var claims = new[] { new Claim("userId", "5") }; 
            httpContextMock.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(claims)));
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var request = new EditRequest(1, "Title", "Text", new List<string>());

            var ex = await Assert.ThrowsAsync<BlogTalksException>(
                () => _handler.Handle(request, CancellationToken.None));

            Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
        }
    }
}
