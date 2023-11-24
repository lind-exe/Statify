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
            int timeoutMs = 1000; 

            var timeout = new CancellationTokenSource();
            timeout.CancelAfter(timeoutMs);

            try
            {
                var regex = new Regex(pattern);
                return regex.Replace(input, replacement);
            }
            catch (OperationCanceledException)
            {
                return "Timeout occurred during regex operation";
            }
        }
    }
}