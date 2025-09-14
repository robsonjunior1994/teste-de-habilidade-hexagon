using Hexagon.Api.Domain.Models;
using Hexagon.Api.Domain.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hexagon.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly JwtService _jwtService;
        private readonly User _testUser;

        public JwtServiceTests()
        {
            // Configuração mockada
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Secret"]).Returns("minha-chave-super-secreta-32-caracteres");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("test-audience");
            _configurationMock.Setup(c => c["Jwt:ExpiresInHours"]).Returns("2");

            _jwtService = new JwtService(_configurationMock.Object);

            _testUser = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Password = "hashedpassword"
            };
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidToken()
        {
            // Act
            var token = _jwtService.GenerateToken(_testUser);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // Verifica se é um JWT válido
            var jwtHandler = new JwtSecurityTokenHandler();
            Assert.True(jwtHandler.CanReadToken(token));
        }

        [Fact]
        public void GenerateToken_ShouldContainCorrectClaims()
        {
            // Act
            var token = _jwtService.GenerateToken(_testUser);
            var principal = _jwtService.ValidateToken(token);

            // Assert
            Assert.NotNull(principal);
            Assert.Equal(_testUser.Id.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal(_testUser.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
            Assert.Equal(_testUser.Name, principal.FindFirst(ClaimTypes.Name)?.Value);
            Assert.NotNull(principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value);
        }

        [Fact]
        public void ValidateToken_ShouldReturnPrincipalForValidToken()
        {
            // Arrange
            var token = _jwtService.GenerateToken(_testUser);

            // Act
            var principal = _jwtService.ValidateToken(token);

            // Assert
            Assert.NotNull(principal);
            Assert.True(principal.Identity?.IsAuthenticated);
        }

        [Fact]
        public void ValidateToken_ShouldReturnNullForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalid-token-here";

            // Act
            var principal = _jwtService.ValidateToken(invalidToken);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void IsTokenValid_ShouldReturnTrueForValidToken()
        {
            // Arrange
            var token = _jwtService.GenerateToken(_testUser);

            // Act
            var isValid = _jwtService.IsTokenValid(token);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsTokenValid_ShouldReturnFalseForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalid-token-here";

            // Act
            var isValid = _jwtService.IsTokenValid(invalidToken);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void GetUserIdFromToken_ShouldReturnCorrectUserId()
        {
            // Arrange
            var token = _jwtService.GenerateToken(_testUser);

            // Act
            var userId = _jwtService.GetUserIdFromToken(token);

            // Assert
            Assert.Equal(_testUser.Id.ToString(), userId);
        }

        [Fact]
        public void GetUserEmailFromToken_ShouldReturnCorrectEmail()
        {
            // Arrange
            var token = _jwtService.GenerateToken(_testUser);

            // Act
            var email = _jwtService.GetUserEmailFromToken(token);

            // Assert
            Assert.Equal(_testUser.Email, email);
        }

        [Fact]
        public void GetUserNameFromToken_ShouldReturnCorrectName()
        {
            // Arrange
            var token = _jwtService.GenerateToken(_testUser);

            // Act
            var name = _jwtService.GetUserNameFromToken(token);

            // Assert
            Assert.Equal(_testUser.Name, name);
        }

        [Fact]
        public void GetMethods_ShouldReturnNullForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalid-token-here";

            // Act & Assert
            Assert.Null(_jwtService.GetUserIdFromToken(invalidToken));
            Assert.Null(_jwtService.GetUserEmailFromToken(invalidToken));
            Assert.Null(_jwtService.GetUserNameFromToken(invalidToken));
        }
    }
}

