using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class ExternalIds
    {

        [JsonPropertyName("isrc")]
        public string? Isrc { get; set; }

        [JsonPropertyName("ean")]
        public string? Ean { get; set; }

        [JsonPropertyName("upc")]
        public string? Upc { get; set; }
    }

}
