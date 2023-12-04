using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class TrackData
    {
        public class BaseResponse
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
            public object? Previous { get; set; }

            [JsonPropertyName("total")]
            public int Total { get; set; }
        }
        public class TrackList : BaseResponse
        {
            [JsonPropertyName("items")]
            public List<Track>? Tracks { get; set; }
        }
        public class LikedTracks : BaseResponse
        {
            //[JsonPropertyName("previous")]
            //public string? Previous { get; set; }

            [JsonPropertyName("items")]
            public List<TrackItem>? Tracks { get; set; }
        }
        public class TrackItem
        {
            [JsonPropertyName("added_at")]
            public string? AddedAt { get; set; }

            [JsonPropertyName("track")]
            public Track? Track { get; set; }
        }
    }
}
