//using BlogTalks.Application.BlogPosts.Queries;
//using BlogTalks.Domain.Entities;
//using BlogTalks.Domain.Exceptions.BlogTalks.Domain.Exceptions;
//using BlogTalks.Domain.Reposotories;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace BlogTalksProjectTests.BlogPosts
//{
//    public class GetBlogPostByIdHandlerTests
//    {
//        private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
//        private readonly GetByIdHandler _handler;

//        public GetBlogPostByIdHandlerTests()
//        {
//            _blogPostRepositoryMock = new Mock<IBlogPostRepository>();
//            _handler = new GetByIdHandler(_blogPostRepositoryMock.Object);
//        }

//        [Fact]
//        public async Task Handler_BlogPostExists_ReturnBlogPostResponse()
//        {
            
//            var expectedId = 1;
//            var expectedBlogPost = new BlogPost
//            {
//                Id = expectedId,
//                Title = "Title",
//                Text = "Text",
//                Tags = new List<string> { "Tag1" },
//                CreatedAt = DateTime.Now,
//                CreatedBy = 5
//            };

//            _blogPostRepositoryMock
//                .Setup(repo => repo.GetById(expectedId))
//                .Returns(expectedBlogPost);

//            var query = new GetByIdRequest(expectedId);

//            var result = await _handler.Handle(query, CancellationToken.None);

//            Assert.NotNull(result);
//            Assert.Equal(expectedId, result.Id);
//            Assert.Equal(expectedBlogPost.Title, result.Title);
//        }
//        [Fact]
//        public async Task Handle_BlogPostNotExist_ReturnsException()
//        {
//            var expectedId = 1;
//            var query = new GetByIdRequest(expectedId);

//            await Assert.ThrowsAsync<BlogTalksException>(() => _handler.Handle(query, CancellationToken.None));
//        }
//    }
//}
