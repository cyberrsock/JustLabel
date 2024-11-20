using System;
using System.Security.Cryptography;
using System.Text;

namespace JustLabel.Utilities;

public class SaltedHash
{
    private const int SaltSize = 16;

    public static string GenerateSaltedHash(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        byte[] saltedPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];

        Buffer.BlockCopy(saltBytes, 0, saltedPasswordBytes, 0, saltBytes.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, saltBytes.Length, passwordBytes.Length);

        using (var sha1 = SHA1.Create())
        {
            byte[] hashBytes = sha1.ComputeHash(saltedPasswordBytes);
            byte[] saltedHashBytes = new byte[hashBytes.Length + saltBytes.Length];

            Buffer.BlockCopy(hashBytes, 0, saltedHashBytes, 0, hashBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, saltedHashBytes, hashBytes.Length, saltBytes.Length);

            return Convert.ToBase64String(saltedHashBytes);
        }
    }

    public static bool VerifySaltedHash(string password, string salt, string hash)
    {
        string expectedHash = GenerateSaltedHash(password, salt);
        return hash.Equals(expectedHash);
    }

    public static string GenerateSalt()
    {
        byte[] saltBytes = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
}

