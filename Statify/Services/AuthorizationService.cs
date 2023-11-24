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
            const string allowedChars = "abcdefghijklmnopqrstuvwxyz123456789";
            byte[] randomBytes = new byte[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            StringBuilder result = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                result.Append(allowedChars[b % allowedChars.Length]);
            }

            return result.ToString();
        }
        public string GenerateCodeChallenge(string codeVerifier)
        {
            if (!string.IsNullOrWhiteSpace(codeVerifier))
            {
                using var sha256 = SHA256.Create();
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                var b64Hash = Convert.ToBase64String(hash);
                var code = ReplaceWithTimeout(b64Hash, "\\+", "-");
                code = ReplaceWithTimeout(code, "\\/", "_");
                code = ReplaceWithTimeout(code, "=+$", "");
                return code;
            }
            else
            {
                return string.Empty;
            }
        }

        public string ReplaceWithTimeout(string input, string pattern, string replacement)
        {
            int timeoutMs = 5000;
            DateTime startTime = DateTime.Now;

            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
            string result = string.Empty;

            int chunkSize = 100;

            for (int startIndex = 0; startIndex < input.Length; startIndex += chunkSize)
            {
                int endIndex = Math.Min(startIndex + chunkSize, input.Length);
                string chunk = input.Substring(startIndex, endIndex - startIndex);

                result += regex.Replace(chunk, replacement);

                if ((DateTime.Now - startTime).TotalMilliseconds > timeoutMs)
                {
                    result = "Timeout occurred during regex operation";
                    break;
                }
            }
            return result;
        }
    }
}