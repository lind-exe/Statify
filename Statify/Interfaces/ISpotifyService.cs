namespace Statify.Interfaces
{
    public interface ISpotifyService
    {
        public Task<T> SendSpotifyApiRequest<T>(string endpoint);

        public string AuthorizeUser();
        public Task ExchangeCodeForTokenAsync(string code);
    }
}