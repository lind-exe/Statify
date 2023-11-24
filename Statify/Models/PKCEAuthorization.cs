using System.Security.Cryptography;
using System.Text.Json.Serialization;
namespace Statify.Models
{
    public class PKCEAuthorization
    {
        public bool Authenticated { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get;  set; }
    }
}
