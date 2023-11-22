using Statify.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Statify.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public string GenerateRandomString(int length)
        {
            const string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            byte[] values = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(values);
            }

            StringBuilder sb = new StringBuilder(length);
            foreach (byte x in values)
            {
                sb.Append(possible[x % possible.Length]);
            }

            return sb.ToString();
        }
        public string ComputeSHA256(string plain)  // async?
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plain));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}