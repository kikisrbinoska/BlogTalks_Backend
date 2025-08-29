using BlogTalks.Application.Abstractions;
using BlogTalks.Application.User.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Reposotories;
using BlogTalks.Domain.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace BlogTalksProjectTests.Users
{
    public class LoginHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthService> _authServiceMock;

        public LoginHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authServiceMock = new Mock<IAuthService>();
        }

        [Fact]
        public async Task LoginHandler_ShouldReturnFailuireMessage_WhenUsernameIsNotFound()
        {
            var command = new LoginRequest("kiki@gmail.com", "kiki1234", "Test123");

            _userRepositoryMock
                .Setup(r => r.GetByUsername(command.Username))
                .Returns((User)null);
            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                 .Returns(new User
                    {
                       Email = command.Email,
                       Username = "otherUser",
                       Password = PasswordHasher.HashPassword("anyPassword")
                    });


            var handler = new LoginHandler(_userRepositoryMock.Object, _authServiceMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be($"User with username '{command.Username}' not found");
        }

        [Fact]
        public async Task LoginHandler_ShouldReturnSuccessMessage_WhenUsernameIsFound()
        {
            var command = new LoginRequest("kiki@gmail.com", "kiki123", "Test123");

            var user = new User
            {
                Username = command.Username,
                Email = command.Email,
                Password = PasswordHasher.HashPassword(command.Password) 
            };

            _userRepositoryMock
                .Setup(r => r.GetByUsername(command.Username))
                .Returns(user);

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .Returns(user);

            _authServiceMock
                .Setup(a => a.Create(user))
                .Returns("");

            var handler = new LoginHandler(_userRepositoryMock.Object, _authServiceMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Login successful");
            result.Token.Should().Be("");
        }
        [Fact]
        public async Task LoginHandler_ShouldReturnFailureMessage_WhenEmailIsNotFound()
        {
            var command = new LoginRequest("kiki4@gmail.com", "kiki1234", "Test123");

            _userRepositoryMock
                .Setup(r => r.GetByUsername(command.Username))
                .Returns(new User
                {
                    Username = command.Username,
                    Email = "other@example.com",
                    Password = PasswordHasher.HashPassword("anyPassword")
                });

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .Returns((User)null);

            var handler = new LoginHandler(_userRepositoryMock.Object, _authServiceMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be($"User with email '{command.Email}' not found");
        }

        [Fact]
        public async Task LoginHandler_ShouldReturnSuccessMessage_WhenEmailIsFound()
        {
            var command = new LoginRequest("kiki@gmail.com", "Test123", "kiki");

            var user = new User
            {
                Username = command.Username,
                Email = command.Email,
                Password = PasswordHasher.HashPassword(command.Password)
            };

            _userRepositoryMock
                .Setup(r => r.GetByUsername(command.Username))
                .Returns(user);

            _userRepositoryMock
                .Setup(r => r.GetByEmail(command.Email))
                .Returns(user);

            _authServiceMock
                .Setup(a => a.Create(user))
                .Returns("mocked_token");

            var handler = new LoginHandler(_userRepositoryMock.Object, _authServiceMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Login successful");
            result.Token.Should().Be("mocked_token");
        }


    }
}
