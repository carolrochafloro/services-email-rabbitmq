using System.Security.Cryptography;

namespace FormContato.Services;

public class EncryptedRecipientEmail
{
    public string EncryptedEmail { get; set; }
    public string DecryptKey { get; set; }
    public string DecryptIV { get; set; }

    public async Task<EncryptedRecipientEmail> GenUrl(string email)
    {

        byte[] encrypted;

        // string to bytes + encrypt
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
                        swEncrypt.Write(email);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }

            string encryptedBase64 = Convert.ToBase64String(encrypted);

            return new EncryptedRecipientEmail
            {
                EncryptedEmail = encryptedBase64,
                DecryptKey = DecryptKey,
                DecryptIV = DecryptIV
            };

        }

    }
}
