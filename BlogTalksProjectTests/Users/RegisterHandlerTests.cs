using BlogTalks.Application.User.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Reposotories;
using Moq;
using Xunit;
using FluentAssertions;

namespace BlogTalksProjectTests.Users
{
    public class RegisterHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public RegisterHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureMessage_WhenEmailIsNotUnique()
        {
            var command = new RegisterRequest("kiki@gmail.com", "kiki123", "Kiki", "Test123.");
            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .Returns(new User { Email = command.Email });

            var handler = new RegisterHandler(_userRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Message.Should().Be($"User with mail {command.Email} already exist");
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureMessage_WhenUsernameIsNotUnique()
        {
            var command = new RegisterRequest("kiki@gmail.com", "kiki123", "Kiki", "Test123.");
            _userRepositoryMock
                .Setup(r => r.GetByUsername(command.Username))
                .Returns(new User { Username = command.Username });

            var handler = new RegisterHandler(_userRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Message.Should().Be($"User with username  {command.Username} already exists");
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessMessage_WhenEmailAndUsernameAreUnique()
        {
            var command = new RegisterRequest("unique@gmail.com", "unique123", "UniqueUser", "Test123.");

            var handler = new RegisterHandler(_userRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Message.Should().Be("Register successfull");
        }
        [Fact]
        public async Task Handle_Should_ReturnSuccessMessage_WhenUsernameIsUnique()
        {
            var command = new RegisterRequest("kiki@gmail.com", "kiki123", "Kiki", "Test123.");

            _userRepositoryMock
                .Setup(r => r.GetByUsername(command.Username))
                .Returns((User)null);

            var handler = new RegisterHandler(_userRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Message.Should().Be("Register successfull");
        }
        [Fact]
        public async Task Handle_Should_ReturnSuccessMessage_WhenEmailIsUnique()
        {
            var command = new RegisterRequest("kiki@gmail.com", "kiki123", "Kiki", "Test123.");

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .Returns((User)null);

            var handler = new RegisterHandler(_userRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Message.Should().Be("Register successfull");
        }

    }
}
