using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class Artist
    {
        [JsonPropertyName("external_urls")]
        public ExternalUrls? ExternalUrls { get; set; }

        [JsonPropertyName("followers")]
        public Followers? Followers { get; set; }

        [JsonPropertyName("genres")]
        public List<string>? Genres { get; set; }

        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("images")]
        public List<Image>? Images { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("popularity")]
        public int Popularity { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }
}
