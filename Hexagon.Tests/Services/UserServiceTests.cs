using Hexagon.Api.Domain.Models;
using Hexagon.Api.Domain.Services;
using Hexagon.Api.Domain.Services.Interfaces;
using Hexagon.Api.Infrastruture.Repository.Interface;
using Hexagon.Api.Presentation.Common;
using Hexagon.Api.Presentation.DTOs.Request;
using Moq;

namespace Hexagon.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEncryptionPasswordService> _encryptionServiceMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _encryptionServiceMock = new Mock<IEncryptionPasswordService>();
            _jwtServiceMock = new Mock<IJwtService>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _encryptionServiceMock.Object,
                _jwtServiceMock.Object
            );
        }

        [Fact]
        public async Task Create_ShouldReturnSuccess_WhenUserDoesNotExist()
        {
            // Arrange
            var userDto = new UserRequestDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123"
            };

            _userRepositoryMock.Setup(r => r.GetByEmail(userDto.Email))
                .ReturnsAsync(default(User));

            _encryptionServiceMock.Setup(e => e.EncryptPassword(userDto.Password))
                .Returns("encryptedPassword");

            _userRepositoryMock.Setup(r => r.Create(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.Create(userDto);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Exactly(1));
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(userDto.Name, result.Data.Name);
            Assert.Equal(userDto.Email, result.Data.Email);
            Assert.Equal("encryptedPassword", result.Data.Password);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task Create_ShouldReturnFailure_WhenUserAlreadyExists()
        {
            // Arrange
            var userDto = new UserRequestDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123"
            };

            var existingUser = new User { Email = userDto.Email };
            _userRepositoryMock.Setup(r => r.GetByEmail(userDto.Email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.Create(userDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.RESOURCE_ALREADY_EXISTS, result.ErrorCode);
            Assert.Equal("Username or password is incorrect", result.ErrorMessage);
            Assert.Null(result.Data);
            _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);

        }

        [Fact]
        public async Task Create_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            // Arrange
            var userDto = new UserRequestDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123"
            };

            _userRepositoryMock.Setup(r => r.GetByEmail(userDto.Email))
                .ReturnsAsync(default(User));

            _encryptionServiceMock.Setup(e => e.EncryptPassword(userDto.Password))
                .Returns("encryptedPassword");

            _userRepositoryMock.Setup(r => r.Create(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userService.Create(userDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.DATABASE_ERROR, result.ErrorCode);
            Assert.Equal("An error occurred while creating the user.", result.ErrorMessage);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Login_ShouldReturnSuccess_WithValidCredentials()
        {
            // Arrange
            var loginDto = new LoginRequestDTO
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = loginDto.Email,
                Password = "encryptedPassword"
            };

            _userRepositoryMock.Setup(r => r.GetByEmail(loginDto.Email))
                .ReturnsAsync(user);

            _encryptionServiceMock.Setup(e => e.ValidatePassword(loginDto.Password, user.Password))
                .Returns(true);

            _jwtServiceMock.Setup(j => j.GenerateToken(user))
                .Returns("fake-jwt-token");

            // Act
            var result = await _userService.Login(loginDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("fake-jwt-token", result.Data);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task Login_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var loginDto = new LoginRequestDTO
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            _userRepositoryMock.Setup(r => r.GetByEmail(loginDto.Email))
                .ReturnsAsync(default(User));

            // Act
            var result = await _userService.Login(loginDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.RESOURCE_NOT_FOUND, result.ErrorCode);
            Assert.Equal("User not found", result.ErrorMessage);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Login_ShouldReturnFailure_WhenPasswordIsInvalid()
        {
            // Arrange
            var loginDto = new LoginRequestDTO
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = loginDto.Email,
                Password = "encryptedPassword"
            };

            _userRepositoryMock.Setup(r => r.GetByEmail(loginDto.Email))
                .ReturnsAsync(user);

            _encryptionServiceMock.Setup(e => e.ValidatePassword(loginDto.Password, user.Password))
                .Returns(false);

            // Act
            var result = await _userService.Login(loginDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.INVALID_CREDENTIALS, result.ErrorCode);
            Assert.Equal("Invalid email or password.", result.ErrorMessage);
            Assert.Null(result.Data);
        }
    }
}