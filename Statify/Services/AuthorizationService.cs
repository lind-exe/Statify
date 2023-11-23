using Statify.Interfaces;
using Statify.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Statify.Services
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private int GenerateRandomStringLength = 128;
        public void GenerateCodeChallenge()
        {
            SpotifyAPICodes.CodeVerifier = GenerateRandomString(GenerateRandomStringLength);
            SpotifyAPICodes.CodeChallenge = GenerateCodeChallenge(SpotifyAPICodes.CodeVerifier);
        }
        public string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            var random = new Random();
            var nonce = new char[length];
            for (int i = 0; i < nonce.Length; i++)
            {
                nonce[i] = chars[random.Next(chars.Length)];
            }

            return new string(nonce);
        }
        public string GenerateCodeChallenge(string codeVerifier)
        {
            if (!string.IsNullOrWhiteSpace(codeVerifier))
            {
                using var sha256 = SHA256.Create();
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                var b64Hash = Convert.ToBase64String(hash);
                var code = Regex.Replace(b64Hash, "\\+", "-");
                code = Regex.Replace(code, "\\/", "_");
                code = Regex.Replace(code, "=+$", "");
                return code;
            }
            else
                return string.Empty;
        }

    }
}