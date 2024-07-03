using FluentAssertions;
using FormContato.Services;

namespace FormContato.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void HashPassword_ShouldSetPasswordAndSalt()
    {
        // Arrange
        var service = new PasswordHasher();
        string password = "123456";

        // Act
        service.HashPassword(password);

        // Assert
        service.Password.Should().NotBeNull();
        service.Salt.Should().NotBeNull();
    }

    [Fact]
    public void IsValidPassword_ShouldReturnTrue_ForValidPassword()
    {
        // Arrange
        var service = new PasswordHasher();
        string password = "123456";
        service.HashPassword(password);
        string storedHash = service.Password;
        string salt = service.Salt;

        // Act
        bool result = service.IsValidPassword(password, salt, storedHash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidPassword_ShouldReturnFalse_ForInvalidPassword()
    {
        // Arrange
        var service = new PasswordHasher();
        string password = "123456";
        service.HashPassword(password);
        string invalidHash = "000111";

        // Act
        bool result = service.IsValidPassword(password, service.Salt, invalidHash);

        // Assert
        result.Should().BeFalse();
    }
}
