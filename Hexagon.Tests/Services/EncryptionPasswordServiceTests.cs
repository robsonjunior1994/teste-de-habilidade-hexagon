using Hexagon.Api.Domain.Services;

namespace Hexagon.Tests.Services
{
    public class PasswordEncryptionServiceTests
    {
        private readonly EncryptionPasswordService _service;

        public PasswordEncryptionServiceTests()
        {
            // Arrange comum para todos os testes
            _service = new EncryptionPasswordService();
        }

        [Fact]
        public void EncryptPassword_ValidPassword_ReturnsNonEmptyString()
        {
            // Arrange
            var password = "minhaSenhaSuperSegura123";

            // Act
            var result = _service.EncryptPassword(password);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void EncryptPassword_SamePassword_GeneratesDifferentHashes()
        {
            // Arrange
            var password = "senhaIdentica";

            // Act
            var hash1 = _service.EncryptPassword(password);
            var hash2 = _service.EncryptPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void ValidatePassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "SenhaCorreta@2024";
            var encryptedPassword = _service.EncryptPassword(password);

            // Act
            var result = _service.ValidatePassword(password, encryptedPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var correctPassword = "SenhaCorreta";
            var wrongPassword = "SenhaErrada";
            var encryptedPassword = _service.EncryptPassword(correctPassword);

            // Act
            var result = _service.ValidatePassword(wrongPassword, encryptedPassword);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FullFlow_EncryptAndValidate_WorksCorrectly()
        {
            // Arrange
            var originalPassword = "UsuarioSenha123";

            // Act - Fluxo completo
            var encrypted = _service.EncryptPassword(originalPassword);
            var validationResult = _service.ValidatePassword(originalPassword, encrypted);

            // Assert
            Assert.True(validationResult);
        }

        [Fact]
        public void EncryptPassword_GeneratedHash_HasCorrectSize()
        {
            // Arrange
            var password = "testeTamanho";

            // Act
            var hashString = _service.EncryptPassword(password);
            var hashBytes = Convert.FromBase64String(hashString);

            // Assert
            Assert.Equal(16 + 32, hashBytes.Length);
        }
    }
}