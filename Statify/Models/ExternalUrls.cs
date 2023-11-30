using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class ExternalUrls
    {
        [JsonPropertyName("spotify")]
        public string? Spotify { get; set; }
    }
}
