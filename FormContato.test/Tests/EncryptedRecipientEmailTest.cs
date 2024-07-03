using FluentAssertions;
using FormContato.Services;
using System.Security.Cryptography;


namespace FormContato.Tests;

public class EncryptedRecipientEmailTests
{
    [Fact]
    public async Task Encrypt_ShouldReturnEncryptedEmail()
    {
        // Arrange
        var service = new EncryptedRecipientEmail();
        var email = "test@example.com";
        var userId = "12345";

        // Act
        var result = await service.Encrypt(email, userId);

        // Assert

        result.EncryptedEmail.Should().NotBeNull();
        result.DecryptKey.Should().NotBeNull();
        result.DecryptIV.Should().NotBeNull();
        result.DecryptKey.Should().Be(service.DecryptKey);
        result.DecryptIV.Should().Be(service.DecryptIV);
    }

    [Fact]
    public async Task Encrypt_ShouldEncryptCorrectly()
    {
        // Arrange
        var service = new EncryptedRecipientEmail();
        var email = "test@example.com";
        var userId = "12345";

        // Act
        var result = await service.Encrypt(email, userId);

        // Restore Base64 string to original format
        var encryptedEmail = result.EncryptedEmail
            .Replace('-', '+')
            .Replace('_', '/');

        // Add padding if needed
        switch (encryptedEmail.Length % 4)
        {
            case 2: encryptedEmail += "=="; break;
            case 3: encryptedEmail += "="; break;
        }

        // Assert
        var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(result.DecryptKey);
        aes.IV = Convert.FromBase64String(result.DecryptIV);

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedEmail)))
        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        using (var srDecrypt = new StreamReader(csDecrypt))
        {
            var decrypted = await srDecrypt.ReadToEndAsync();
            decrypted.Should().Be(email + userId);
        }
    }
}


