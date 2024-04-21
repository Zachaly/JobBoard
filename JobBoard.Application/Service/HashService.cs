using JobBoard.Application.Service.Abstraction;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace JobBoard.Application.Service
{
    public class HashService : IHashService
    {
        private readonly string _secretString;
        private const int KeyIterations = 1000;

        public HashService(IConfiguration config) 
        {
            _secretString = config["PasswordSalt"]!;
        }

        public string HashPassword(string password)
        {
            var salt = Encoding.UTF8.GetBytes(_secretString);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                KeyIterations,
                HashAlgorithmName.SHA512,
                salt.Length
                );

            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var salt = Encoding.UTF8.GetBytes(_secretString);

            var compareHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                KeyIterations,
                HashAlgorithmName.SHA512,
                salt.Length
                );

            return CryptographicOperations.FixedTimeEquals(compareHash, Convert.FromHexString(hash));
        }
    }
}
