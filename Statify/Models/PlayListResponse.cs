using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class PlaylistResponse
    {
        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("previous")]
        public string? Previous { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("items")]
        public List<PlaylistItem>? Items { get; set; }
    }
    public class PlaylistItem
    {
        [JsonPropertyName("collaborative")]
        public bool Collaborative { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls? ExternalUrls { get; set; }

        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("images")]
        public List<Image>? Images { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("owner")]
        public Owner? Owner { get; set; }

        [JsonPropertyName("public")]
        public bool Public { get; set; }

        [JsonPropertyName("snapshot_id")]
        public string? SnapshotId { get; set; }

        [JsonPropertyName("tracks")]
        public Tracks? Tracks { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }
    public class Owner
    {
        [JsonPropertyName("external_urls")]
        public ExternalUrls? ExternalUrls { get; set; }

        [JsonPropertyName("followers")]
        public Followers? Followers { get; set; }

        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }
    }
    public class Tracks
    {
        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
