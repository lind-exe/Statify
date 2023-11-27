using Statify.Models;
using Statify.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Statify.Services
{
    public class UserService : IUserService
    {
        public PKCEAuthorization? Authentication { get; set; }
        public User? User { get; set; }
        public PlayListCollection? PlaylistCollection { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<T> SendSpotifyApiRequest<T>(string endpoint)
        {
            Authentication = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<PKCEAuthorization>("User");

            if (Authentication is null)
            {
                throw new InvalidOperationException("User authentication is missing.");
            }

            string accessToken = Authentication.AccessToken;

            using HttpClient httpClient = new();
            httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content) ?? throw new NullReferenceException($"{typeof(T).Name} was null after deserialization.");
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve information from Spotify API. Status code: {response.StatusCode}");
            }
        }

        public async Task<User> GetUserFromSpotifyWithWebApi()
        {
            return await SendSpotifyApiRequest<User>("me");
        }

        public async Task<PlayListCollection> Get50PlaylistsFromAuthorizedUser()
        {
            return await SendSpotifyApiRequest<PlayListCollection>("me/playlists");
        }
    }
}
