using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class ArtistData
    {
        public class BaseResponse
        {
            
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
        public class ArtistItems : BaseResponse
        {
            [JsonPropertyName("items")]
            public List<Artist>? Artists { get; set; }
        }
        public class ArtistArtists : BaseResponse
        {
            [JsonPropertyName("artists")]
            public List<Artist>? Artists { get; set; }
        }
    }
}
