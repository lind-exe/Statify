namespace Statify.Interfaces
{
    public interface ISpotifyService
    {
        /// <summary>
        /// Expects type and endpoint for eventual Spotify api request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns>Deserialized generic object</returns>
        public Task<T> SendRequest<T>(string endpoint);
        /// <summary>
        /// Authorizes user by generating code challenge and query parameters
        /// </summary>
        /// <returns>Redirect url and code</returns>
        public string AuthorizeUser();
        /// <summary>
        /// Exchanges code from AuthorizeUser() into an access token that is stored in browser session data
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Access token to session data</returns>
        public Task ExchangeCodeForTokenAsync(string code);
    }
}