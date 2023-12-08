using Statify.Interfaces;
using Statify.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;

namespace Statify.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly int GenerateRandomStringLength = 128;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SpotifyApiCodes SpotifyApiCodes { get; set; }

        public AuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            SpotifyApiCodes = new();
        }

        private const string RedirectUri = "https://localhost:7274";
        private const string Scope = "user-read-private user-read-email user-top-read user-library-read";

        public void GenerateCodeChallenge()
        {
            SpotifyApiCodes.CodeVerifier = GenerateRandomString(GenerateRandomStringLength);
            SpotifyApiCodes.CodeChallenge = GenerateCodeChallenge(SpotifyApiCodes.CodeVerifier!);
            
            _httpContextAccessor.HttpContext!.Session.SetObjectAsJson("SpotifyApiCodes", SpotifyApiCodes);
        }
        public string GenerateRandomString(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyz123456789";
            byte[] randomBytes = new byte[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            StringBuilder result = new(length);
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
                var hash = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
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
                string chunk = input[startIndex..endIndex];

                result += regex.Replace(chunk, replacement);

                if ((DateTime.Now - startTime).TotalMilliseconds > timeoutMs)
                {
                    result = "Timeout occurred during regex operation";
                    break;
                }
            }
            return result;
        }
        public string GenerateQueryParams()
        {
            string authUrl = $"https://accounts.spotify.com/authorize";
            var queryParams = new StringBuilder();
            queryParams.Append($"?response_type=code");
            queryParams.Append($"&client_id={SpotifyApiCodes.ClientId}");
            queryParams.Append($"&scope={Scope}");
            queryParams.Append($"&code_challenge_method=S256");
            queryParams.Append($"&code_challenge={SpotifyApiCodes.CodeChallenge}");
            queryParams.Append($"&redirect_uri={RedirectUri}");

            return authUrl + queryParams.ToString();
        }
    }
}