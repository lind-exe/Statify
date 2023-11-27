namespace Statify.Models
{
    public class SpotifyApiCodes
    {
        public const string ClientId = "de733e5c3f6b418a97c787f8abe82ba5";

        public static string? CodeVerifier { get; private set; }
        public static string? CodeChallenge { get; private set; }

        public static void SetCodeVerifier(string codeVerifier)
        {
            CodeVerifier = codeVerifier;
        }
        public static void SetCodeChallenge(string codeChallenge)
        {
            CodeChallenge = codeChallenge;
        }
    }
}
