using System.Security.Cryptography;

namespace FormContato.Services;

public class EncryptedRecipientEmail
{
    public string? EncryptedEmail { get; set; }
    public string? DecryptKey { get; set; }
    public string? DecryptIV { get; set; }

    public async Task<EncryptedRecipientEmail> Encrypt(string email, string userId)
    {

        byte[] encrypted;

        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.GenerateIV();

            DecryptKey = Convert.ToBase64String(aes.Key);
            DecryptIV = Convert.ToBase64String(aes.IV);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(email + userId);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }

            string encryptedBase64UrlFriendly = Convert.ToBase64String(encrypted).TrimEnd('=').Replace('+', '-').Replace('/', '_');


            return new EncryptedRecipientEmail
            {
                EncryptedEmail = encryptedBase64UrlFriendly,
                DecryptKey = DecryptKey,
                DecryptIV = DecryptIV
            };

        }

    }
}

