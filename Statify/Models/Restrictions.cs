using System.Text.Json.Serialization;

namespace Statify.Models
{
    public class Restrictions
    {
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}
