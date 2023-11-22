using System.Security.Cryptography;
using System.Text.Json.Serialization;
namespace Statify.Models
{
    public class PKCEAuthorization
    {
        [JsonPropertyName("access_token")]
        public int AccessToken { get; private set; }

        [JsonPropertyName("token_type")]
        public int TokenType { get; private set; }

        [JsonPropertyName("scope")]
        public int Scope { get; private set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; private set; }

        [JsonPropertyName("refresh_token")]
        public int RefreshToken { get; private set; }
    }
}
