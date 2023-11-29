namespace Statify.Models
{
    public class SpotifyApiCodes
    {
        public const string ClientId = "de733e5c3f6b418a97c787f8abe82ba5";

        public string? CodeVerifier { get; set; }
        public string? CodeChallenge { get; set; }
    }
}
