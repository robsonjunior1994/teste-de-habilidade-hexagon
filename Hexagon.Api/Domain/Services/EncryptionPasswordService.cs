using Hexagon.Api.Domain.Services.Interfaces;
using System.Security.Cryptography;

namespace Hexagon.Api.Domain.Services
{
    public class EncryptionPasswordService : IEncryptionPasswordService
    {
        private const int HashSize = 32;
        private const int SaltSize = 16;
        private const int Iterations = 10000;

        public string EncryptPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashToStore = new byte[SaltSize + HashSize];

            Array.Copy(salt, 0, hashToStore, 0, SaltSize);

            Array.Copy(hash, 0, hashToStore, SaltSize, HashSize);

            return Convert.ToBase64String(hashToStore);
        }

        public bool ValidatePassword(string openPassword, string encryptedPassword)
        {
            var hashBytes = Convert.FromBase64String(encryptedPassword);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(openPassword, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
