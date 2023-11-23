using System.Security.Cryptography;
using System.Text.Json.Serialization;
namespace Statify.Models
{
    public class PKCEAuthorization
    {
        [JsonPropertyName("access_token")]
        public int AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public int TokenType { get; set; }

        [JsonPropertyName("scope")]
        public int Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public int RefreshToken { get; private set; }
    }
}
