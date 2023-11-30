using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class ArtistCollection
    {
        [JsonPropertyName("items")]
        public List<Artist>? Items { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("previous")]
        public object? Previous { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }
}
