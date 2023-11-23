using Statify.Interfaces;
using Statify.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Statify.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public void GenerateCodeChallenge()
        {
            SpotifyAPICodes.CodeVerifier = GenerateRandomString();
            SpotifyAPICodes.CodeChallenge = GenerateCodeChallenge(SpotifyAPICodes.CodeVerifier);
        }
        public string GenerateRandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            var random = new Random();
            var nonce = new char[128];
            for (int i = 0; i < nonce.Length; i++)
            {
                nonce[i] = chars[random.Next(chars.Length)];
            }

            return new string(nonce);
        }
        public string GenerateCodeChallenge(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }

    }
}