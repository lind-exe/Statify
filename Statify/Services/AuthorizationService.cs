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
        public string Base64EncodeSHA256(string sha256Hash)
        {
            byte[] hashBytes = Encoding.UTF8.GetBytes(sha256Hash);
            string base64String = Convert.ToBase64String(hashBytes)
                .Replace("=", "")
                .Replace("+", "-")
                .Replace("/", "_");

            return base64String;
        }
        public string GenerateCodeChallenge()
        {
            var length = 64;

            var randomString = GenerateRandomString(length);
            var sha256 = ComputeSHA256(randomString);
            var base64 = Base64EncodeSHA256(sha256);

            return base64;
        }
    }
}